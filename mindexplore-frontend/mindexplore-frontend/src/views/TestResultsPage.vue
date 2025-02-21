<template>
  <div class="page">
    <PageTitleBar :title="`Rezultate test pentru ${patientName}` " />
    <div class="page-wrapper">
      <!-- Display test data -->
      <div v-if="test">
        <p v-for="(answer, question) in test.questions" :key="question">
         
            <!-- <v-card>
            <strong>{{ question }}</strong><br> 
            <v-card-title>{{ question }}</v-card-title>
            {{ answer }}
          </v-card> -->
          <v-card v-if="answer !== 'null' && answer != ''" class="answer-card">
              <v-card-title class="card-title">{{ question }}</v-card-title>
              <v-card-text class="card-text">{{ answer }}</v-card-text>
            </v-card>
          
         
        </p>
      
        <v-card class="screening"  variant="outlined">
        <strong>Scorul testului PHQ-9: {{ test.depressionScore }}</strong>
        <p v-if="test.depressionScore >= 0 && test.depressionScore <= 4">
          Severitatea indicată este scăzută spre deloc. Se recomandă monitorizare, ar putea să nu necesite tratament.
        </p>
        <p v-else-if="test.depressionScore >= 5 && test.depressionScore <= 9">
          Severitate ușoară. Utilizați raționamentul clinic (durata simptomelor, afectarea funcțională) pentru a determina necesitatea tratamentului.
        </p>
        <p v-else-if="test.depressionScore >= 10 && test.depressionScore <= 14">
          Severitate moderată. Utilizați raționamentul clinic (durata simptomelor, afectarea funcțională) pentru a determina necesitatea tratamentului
        </p>
        <p v-else-if="test.depressionScore >= 15 && test.depressionScore <= 19">
          Severitate moderat severă. Necesită tratament activ cu psihoterapie, medicamente sau combinație
        </p>
        <p v-else-if="test.depressionScore >= 20 && test.depressionScore <= 27">
          Severitate severă. Necesită tratament activ cu psihoterapie, medicamente sau combinație
        </p>
      </v-card>

      <v-card class="screening" variant="outlined">
        <strong>Scorul testului GAD-7: {{ test.anxietyScore }}</strong>
        <p v-if="test.anxietyScore >= 5 && test.anxietyScore <= 9">
          Severitate ușoară. Este necesară monitorizarea.
        </p>
        <p v-else-if="test.anxietyScore >= 10 && test.anxietyScore <= 14">
          Anxietate Moderată. S-ar putea ca rezultatul să fie relevant pentru depistarea altei afecțiuni clinice precum: 
          Tulburare de panică, fobie socială și PTSD
        </p>
        <p v-else-if="test.anxietyScore >= 15 ">
          Anxietate Severă. Necesită tratament activ.
        </p>
        
        
        
      </v-card>
      </div>
      <div v-else>
        Loading test data...
      </div>
    </div>
  </div>
</template>

<script>
import PageTitleBar from '../components/PageTitleBar.vue';
import axios from 'axios';
import { API_BASE } from '@/services/constants';

export default {
  data() {
    return {
      test: null,
      patientName: null
    };
  },
  components: {
    PageTitleBar,
  },
  mounted() {
    this.fetchTest();
    this.fetchPatient();
  },
  computed: {
    token() {
      return localStorage.getItem("token");
    },
  },
  methods: {
    async fetchTest() {
      const patientId = this.$route.params.id;
      try {
        const response = await axios.get(`${API_BASE}/api/Test/${patientId}`);
        this.test = response.data;
      } catch (error) {
        console.error(error);
      }
    },
    async fetchPatient() {
      const patientId = this.$route.params.id;
      const response = await axios.get(`${API_BASE}/api/Patients/${patientId}`,
        {
          headers: {
              'Authorization': 'Bearer ' + this.token,
              Role: 'Therapist',
            },})
      this.patientName = response.data.firstName + ' ' + response.data.lastName;
     
    }
  },
};
</script>
<style scoped>
.page-wrapper {
  /* margin: 10px; */
}
.card-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  margin-bottom: 20px;
}
.answer-card {
  width: 100%;
  max-width: 70vw;
  margin: 10px;
  
}
.card-title {
  font-weight: bold;
}

.screening {
  padding: 10px;
  margin-left: 10px;
  
}
/* 
.card-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  margin-bottom: 20px;
}

.answer-card {
  width: 100%;
  max-width: 400px;
  margin: 10px;
  text-align: center;
}

.result-card {
  margin-top: 20px;
  text-align: center;
}

.card-title {
  font-weight: bold;
}

.card-text {
  margin-bottom: 10px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
} */

</style>

