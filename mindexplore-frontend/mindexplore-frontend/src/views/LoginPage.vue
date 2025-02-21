<template>
  <div class="login-page">
    <div class="login-wrapper">
      <v-card style="width:calc(min(50%, 400px))">
        <v-card-title class="font-weight-bold ma-2">Autentificare</v-card-title>
        <v-card-text>
          <v-form @submit.prevent="login">
            <v-text-field v-model="email" label="Email" required></v-text-field>
            <v-text-field v-model="password" label="Parolă" type="password" required></v-text-field>
            <div style="display: flex; justify-content: space-between;">
              <v-btn type="submit" color="primary">Conectare</v-btn>
              <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
              <p>Nu ai încă un cont?
                <v-btn type="submit" variant="outlined" to="/register" color="info">Nu</v-btn>
              </p>
              
            </div>
          </v-form>
        </v-card-text>
      </v-card>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import {useUserStore} from '@/store/user';
import {mapState} from 'pinia';
import {API_BASE} from '@/services/constants';
// import { useJwt } from '@vueuse/integrations/useJwt'

export default {
  components: {},
  data() {
    return {
      email: '',
      password: '',
      errorMessage: ''
    };
  },
  computed: {
    ...mapState(useUserStore, ['user', 'token'])
  },
  methods: {
    parseJwt(token) {
      try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const decodedToken = JSON.parse(window.atob(base64));
        return decodedToken;
      } catch (error) {
        console.error('Failed to parse JWT token:', error);
        return null;
      }
    },
    login() {
      this.errorMessage = '';
      // Get user login data from form inputs
      const userData = {
        Email: this.email,
        Password: this.password
      };

      // Send POST request to login endpoint
      axios.post(`${API_BASE}/api/Auth/Login`, userData)
          .then(async response => {
            // Handle successful login response
           // console.log('Login successful:', response.data);

            // Perform any necessary actions, such as updating UI or storing authentication token
            const accessToken = response.data.accessToken;
           // console.log(accessToken)
            const decodedToken = this.parseJwt(accessToken);
            //console.log(decodedToken.nameid);

            //getting the name based on the userId in token
            const userDetailsResponse = await axios.get(`${API_BASE}/api/User/${decodedToken.nameid}/details`);
            const name = userDetailsResponse.data.firstName + " " + userDetailsResponse.data.lastName;
            console.log(userDetailsResponse.data.firstName);

            let userId = await this.fetchReceiverUserId(decodedToken.role, decodedToken.nameid)
            const userStore = useUserStore();
            userStore.login(response.data.accessToken, decodedToken.role, name, decodedToken.nameid, userId);
            this.$router.push("/")

          })
          .catch(error => {
            // Handle login error
            console.error('Login failed:', error);
            // Perform error handling, such as showing an error message
            this.errorMessage = 'Email sau parola incorecta';
          });
    },
    async fetchReceiverUserId(role, userId) {
      let response = await axios.get(`${API_BASE}/api/User/${userId}/${role}`)
      return response.data
    },
  }
}
</script>

<style scoped lang="scss">
.error-message {
  color:red;
}
.login-page {
  min-height: 100%;
  width: 100%;
  display: flex;
  flex-direction: column;
  background-color: #af9ad6;
}

.login-wrapper {
  flex-grow: 1;
  place-content: center;
  place-items: center;
  display: flex;
}


.login-form {
  width: min(300px, 50%);
  background-color: rgba(255, 255, 255, 0.13);
  border-radius: 10px;
  backdrop-filter: blur(10px);
  border: 2px solid rgba(255, 255, 255, 0.1);
  box-shadow: 0 0 40px rgba(8, 7, 16, 0.6);
  padding: 35px;

  button {
    margin-top: 50px;
    width: 100%;
    background-color: #b0b0b0;
    color: #080710;
    padding: 15px 0;
    font-size: 18px;
    font-weight: 600;
    border-radius: 5px;
    cursor: pointer;
  }

  input {
    display: block;
    height: 50px;
    border: 1px solid orange;
    width: 100%;
    max-width: 100%;
    background-color: rgba(255, 255, 255, 0.07);
    border-radius: 3px;
    padding: 0 10px;
    margin-top: 8px;
    font-size: 14px;
    font-weight: 300;
  }
  
}
</style>
