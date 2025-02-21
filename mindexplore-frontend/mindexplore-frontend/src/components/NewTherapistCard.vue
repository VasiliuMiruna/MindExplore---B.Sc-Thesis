<template>
    <v-card>
      <div class="d-flex flex-no-wrap justify-space-between pa-5">
        <div>
          <v-card-title class="text-h5">
            {{ therapistCard.firstName }} {{ therapistCard.lastName }}
          </v-card-title>
  
          <v-card-subtitle>"{{ therapistCard.quote || 'Ai grija de tine si de mintea ta!' }}"</v-card-subtitle>
          <v-card-subtitle>
  
            <v-icon class="ml-1" color="primary">mdi-star</v-icon>
            <span>Rating:{{ ratingWithOneDecimal }}</span>
          </v-card-subtitle>
          <v-card-subtitle>Pret: {{ therapistCard.price }} lei/sedinta</v-card-subtitle>
          <v-card-subtitle>Ore practica:+ {{ therapistCard.noHoursPractice }}</v-card-subtitle>
          <v-card-actions>
            <v-btn color="green" @click="addTherapist" prepend-icon="fas fa-check" >Adauga</v-btn>
            <v-btn color="red" @click="deleteTherapist" prepend-icon="fas fa-x" >Sterge</v-btn>
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
          <v-img :src="imagePath" class="therapist-photo"></v-img>
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
        specialties: []
      };
    },
    computed: {
      imagePath() {
        return `https://randomuser.me/api/portraits/women/${Math.floor(Math.random() * 100) + 1}.jpg`
      },
      ratingWithOneDecimal() {
        if (this.therapistCard.rating !== null)
          return this.therapistCard.rating.toFixed(1);
        else return "Fara Rating"
      },
      token() {
      return localStorage.getItem("token");
        },
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
      async addTherapist() {
        console.log(this.therapistCard.id)
        try { await axios.put(`${API_BASE}/api/Therapist/${this.therapistCard.id}/add`,
        {
          headers: {
              'Authorization': 'Bearer ' + this.token,
              Role: 'Admin',
            }
        })
        location.reload()
      } catch(error) {
        console.error(error);
      }
      },
      async deleteTherapist() {
        console.log(this.therapistCard.id)
        try { await axios.delete(`${API_BASE}/api/Therapist/${this.therapistCard.id}`,
        {
          headers: {
              'Authorization': 'Bearer ' + this.token,
              Role: 'Admin',
            }
        })
        location.reload()
      } catch(error) {
        console.error(error);
      }
      }
  
    }
  
  };
  </script>
  
  <style>
  
  </style>
  