<template>
    <v-card>
      <div class="d-flex flex-no-wrap justify-space-between pa-5">
        <div>
          <v-card-title class="text-h5">
            {{ therapistCard.firstName }} {{ therapistCard.lastName }}
          </v-card-title>
  
          <v-card-subtitle>"{{ therapistCard.quote || 'Ai grija de tine si de mintea ta!' }}"</v-card-subtitle>
          <v-card-subtitle>
  
            <v-rating
            v-model="localRating"
            background-color="transparent"
            empty-icon="mdi-star-outline"
            filled-icon="mdi-star"
            dense
            hover
            @click="updateRating"
        ></v-rating>
        <span v-if="therapistCard.rating !== null">Rating: {{ ratingWithOneDecimal }}</span>
        <span v-else>No Rating</span>
          </v-card-subtitle>
          <v-card-subtitle>Pret: {{ therapistCard.price }} lei/sedinta</v-card-subtitle>
          <v-card-subtitle>Oraș: {{ therapistCard.city }} /online</v-card-subtitle>
          <v-card-actions>
            <router-link :to="'/therapist/' + therapistCard.id + '/appointments'">
              <v-btn color="primary" variant="tonal" prepend-icon="mdi-book-clock"
                     class="therapistCard-button">Rezerva
              </v-btn>
            </router-link>
          </v-card-actions>
          <div>
  
            <v-chip
                v-for="specialty in specialties"
                :key="specialty"
                class="ma-1"
                color="primary"
                outlined
                small
            >
              {{ getSpecialtyTranslation(specialty) }}
            </v-chip>
          </div>
  
        </div>
  
        <v-avatar class="ma-3" size="155" rounded="50">
          <v-img :src="imagePath"></v-img>
        </v-avatar>
      </div>
  
    </v-card>
  </template>
  
  <script>
  import axios from 'axios';
  import {API_BASE} from "@/services/constants";
  import specialtiesTranslations from "@/locales/specialties-ro.json";
  
  export default {
    name: "TherapistCard",
    props: ["therapistCard"],
    data() {
      return {
        localRating: 0,
        specialties: []
      };
    },
    computed: {
      imagePath() {
        return `https://randomuser.me/api/portraits/women/${Math.floor(Math.random() * 100) + 1}.jpg`
      },
      token() {
      return localStorage.getItem("token");
        },
      ratingWithOneDecimal() {
        if (this.therapistCard.rating !== null)
          return this.therapistCard.rating.toFixed(1);
        else return "Fara Rating"
      }
    },
    mounted() {
      this.fetchSpecialtiesOfTherapist();
    },
    methods: {
      async fetchSpecialtiesOfTherapist() {
        try {
          const response = await axios.get(`${API_BASE}/api/Therapist/therapistSpecialties/${this.therapistCard.id}`);
          this.specialties = response.data;
        } catch (error) {
          console.error(error);
          this.specialties = [];
        }
      },
      getSpecialtyTranslation(specialty) {
        return specialtiesTranslations.specialties[specialty] || specialty;
      },
      async updateRating() {
        if (this.localRating !== null) {
            // Make sure a rating is selected
            
            try {
            
            await axios.put(`${API_BASE}/api/Therapist/${this.therapistCard.id}/${this.localRating}/rating`,  {
                headers: { 
                    'Authorization': 'Bearer ' + this.token,
                    'Role': 'Patient' 
                } 
            });
            location.reload();
            //this.$emit('rating-updated', this.localRating); // Emit a custom event to notify the parent component
            } catch (error) {
            console.error(error);
            }
        }
    }
}
    
  
  };
  </script>
  
  <style>
  
  </style>
  