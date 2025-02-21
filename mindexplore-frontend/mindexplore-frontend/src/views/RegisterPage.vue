<template>
  <div class="register-page">
    <div class="register-form-wrapper">
      <v-card style="width:calc(min(50%, 400px))">
        <v-card-title class="font-weight-bold ma-2">Creează un cont</v-card-title>
        <v-card-text>
          <v-form @submit.prevent="register">
            <v-row>
              <v-col cols="6" >
                <v-text-field v-model="FirstName" label="Prenume" :rules="firstNameRules" required ></v-text-field>
              </v-col>
              <v-col cols="6">
                <v-text-field v-model="LastName" label="Nume" :rules="lastNameRules" required></v-text-field>
              </v-col>
            </v-row>
            <v-text-field v-model="Email" label="Email" type="email" :rules="emailRules" required></v-text-field>
            <v-text-field v-model="Password" label="Parolă" type="password" :rules="passwordRules" required></v-text-field>

            <div style="display: flex; justify-content: space-between;">
              <v-btn type="submit" color="primary">Creează</v-btn>
              <p>Ai cont deja?
                <v-btn type="submit" variant="outlined" to="/login" color="info">Conectează-te</v-btn>
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

export default {
  data() {
    return {
      FirstName: '',
      LastName: '',
      Email: '',
      Password: '',
      emailRules: [
        value => {
          if (value) return true

          return 'E-mail necesar.'
        },
        value => {
          if (/.+@.+\..+/.test(value)) return true

          return 'E-mail trebuie să fie valid.'
        },
      ],
      firstNameRules: [
        value => {
          if (value) return true

          return 'Prenume necesar.'
        },
      ],
      lastNameRules: [
        value => {
          if (value) return true

          return 'Nume necesar.'
        },
      ],
      passwordRules: [
        value => {
          if (value) return true

          return 'Parolă necesară.'
        },

      ],
    };
  },
  methods: {
    register() {
      const userData = {
        FirstName: this.FirstName,
        LastName: this.LastName,
        Email: this.Email,
        Password: this.Password
      };

      // Send POST request to login endpoint
      axios.post('https://localhost:7032/api/Auth/Register', userData)
          .then(response => {
            // Handle successful login response
            console.log('Register successful:', response.data);
            this.$router.push("/login")
            // Perform any necessary actions, such as updating UI or storing authentication token
          })
          .catch(error => {
            // Handle login error
            console.error('Register failed:', error);
            console.log(userData);
            // Perform error handling, such as showing an error message
          });
    }
  }
}
</script>

<style scoped lang="scss">
.register-page {
  min-height: 100%;
  width: 100%;
  display: flex;
  flex-direction: column;
  background-color: #af9ad6;
}

.register-form-wrapper {
  flex-grow: 1;
  place-content: center;
  place-items: center;
  display: flex;
}


.register-form {
  width: min(400px, 50%);
  background-color: rgba(255, 255, 255, 0.13);
  border-radius: 10px;
  backdrop-filter: blur(10px);
  border: 2px solid rgba(255, 255, 255, 0.1);
  box-shadow: 0 0 40px rgba(8, 7, 16, 0.6);
  padding: 5px 35px;

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
    width: 100%;
    background-color: rgba(255, 255, 255, 0.07);
    border-radius: 3px;
    padding: 0 10px;
    margin-top: 8px;
    font-size: 14px;
    font-weight: 300;
  }
}
</style>
