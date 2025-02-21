<template>
    <v-card>
      <div class="d-flex flex-no-wrap justify-space-between pa-5">
        <div>
        
          <v-card-title class="text-h5">
            <span style=" font-weight: bold;">Nume pacient:</span>
            {{ patientCard.firstName }} {{ patientCard.lastName }}
          </v-card-title>
          <v-card-title> 
            <span style=" font-weight: bold;">Vârsta:</span>
            {{patientCard.age }} </v-card-title>
        </div>
        <v-avatar class="ma-3" size="100" rounded="50">
            <v-img :src="imagePath" class="therapist-photo"></v-img>
          </v-avatar>
    </div>
    <v-card-actions>
      <div class="button-wrapper">
            <router-link :to="{ path: '/therapist/patients/' + patientCard.id + '/conversation', params: { id: patientCard.id } }">
                <v-btn color="primary" variant="tonal" prepend-icon="mdi-message-text"
                    class="conversation-button">Chat
                </v-btn>
            </router-link>
          </div>
          <div class="button-wrapper">
            <router-link :to="{ path: '/' + patientCard.id + '/test', params: { id: patientCard.id } }">
                <v-btn color="primary" variant="tonal" prepend-icon="mdi-book"
                        class="test-button">Chestionar
                </v-btn>
            </router-link>
          </div>
          
    </v-card-actions>
    </v-card>
</template>

<script>
// import axios from 'axios';
// import {API_BASE} from "@/services/constants";

export default {
    name: "PatientCard",
    props: ["patientCard"],
    data() {
        return {

        }
    },
    computed: {
      imagePath() {
      return `https://randomuser.me/api/portraits/women/${Math.floor(Math.random() * 100) + 1}.jpg`
    },
    },
    mounted() {
        console.log(this.patientCard.age);
    },
    methods: {
        // async fetchPatients() {
        //     try {
        //         const response = await axios.get(`${API_BASE}/api/Therapist/${this.patientCard.id}/patients`)

        //     } catch(error) {
        //         console.error(error);
                
        //     }
        // }
    }
}
</script>

<style scoped>
.button-wrapper {
  margin-right: 10px; 
}



</style>