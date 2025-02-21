<template>
  <div class="page">
    <PageTitleBar title="Terapeuți"/>
    <div class="page-wrapper" style="flex-direction: column;">
      <div class="filter-container">
        <v-menu>
          <template v-slot:activator="{ props }">
            <v-btn
                class="mr-5"
                color="white"
                dark
                v-bind="props"
            >
              Sortează
            </v-btn>
          </template>
          <v-list>
            <v-list-item @click="fetchTherapistsByAscendingPrice">
              <v-list-item-title><v-icon class="mr-2">mdi-sort-descending</v-icon>Preț crescător</v-list-item-title>
            </v-list-item>
            <v-list-item @click="fetchTherapistsByDescendingPrice">
              <v-list-item-title><v-icon class="mr-2">mdi-sort-ascending</v-icon>Preț descrescător</v-list-item-title>
            </v-list-item>
          </v-list>
          
        
        </v-menu>
        <v-select
            variant="filled"
            label="Selectează"
            @update:model-value="handleSpecialtyFilter($event)"
            :items="translateSpecialties"
            item-value="id"
            item-title="name"
            multiple
            density="compact"
            hide-details
        ></v-select>
        <!-- <v-btn to="/test" >Găsește un terapeut</v-btn> -->
      </div>
      <section class="therapists">
        <TherapistCard v-for="therapist in therapists" :key="therapist.id" :therapistCard="therapist">
        </TherapistCard>
      </section>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import {API_BASE} from '@/services/constants';
import PageTitleBar from '../components/PageTitleBar.vue';
import TherapistCard from '../components/TherapistCard.vue';
import specialtiesRo from '@/locales/specialties-ro.json';

export default {
  components: {
    PageTitleBar,
    TherapistCard
  },
  data() {
    return {
      therapists: [],
      specialties: []
    };
  },
  async mounted() {
    this.fetchTherapists()
  },
  computed: {
    translateSpecialties() {
      return this.specialties.map(specialty => {
        const translatedName = specialtiesRo.specialties[specialty.name];
        return {
          id: specialty.id,
          name: translatedName || specialty.name
        };
      });
    }
  },
  methods: {
    async fetchTherapists() {
      try {
        const response = await axios.get(`${API_BASE}/api/Therapist`);
        this.therapists = response.data;
        console.log(response.data)
        const specialtiesResponse = await axios.get(`${API_BASE}/api/Specialty/specialties`);
        this.specialties = specialtiesResponse.data;
      } catch (error) {
        console.error(error);
      }
    },
    async handleSpecialtyFilter(values) {
      if (values.length > 0) {
        await this.fetchTherapistsBySpecialty(values);
      } else {
        await this.fetchTherapists();
      }
    },


    async fetchTherapistsByAscendingPrice() {
      try {
        const response = await axios.get(`${API_BASE}/api/Therapist/byAscendingPrice`);
        this.therapists = response.data;
      } catch (error) {
        console.error(error);
      }
    },

    async fetchTherapistsByDescendingPrice() {
      try {
        const response = await axios.get(`${API_BASE}/api/Therapist/byDescendingPrice`);
        this.therapists = response.data;
      } catch (error) {
        console.error(error);
      }
    },

    async fetchTherapistsBySpecialty(specialtyIds) {
      try {
        const response = await axios.get(`${API_BASE}/api/Therapist/bySpecialty`, {
          params: {
            specialtyIds: specialtyIds.join(",")
          }
        });
        this.therapists = response.data;
        console.log("assasas")
        console.log(response.data)
      } catch (error) {
        console.error(error);
      }
    },

    async fetchTherapistsBySpecialtyAndAscendingPrice(specialtyIds) {
      try {
        const response = await axios.get(`${API_BASE}/api/Therapist/bySpecialtyAndAscendingPrice`, {
          params: {
            specialtyIds: specialtyIds
          }
        });
        this.therapists = response.data;
        
      } catch (error) {
        console.error(error);
      }
    },

    async fetchTherapistsBySpecialtyAndDescendingPrice(specialtyIds) {
      try {
        const response = await axios.get(`${API_BASE}/api/Therapist/bySpecialtyAndDescendingPrice`, {
          params: {
            specialtyIds: specialtyIds
          }
        });
        this.therapists = response.data;
      } catch (error) {
        console.error(error);
      }
    },
  }
}
</script>

<style lang="scss" scoped>
.filter-container {
  display: flex;
  align-items: center;
  margin-bottom: 25px;
}

.therapists {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  grid-column-gap: 20px;
  grid-row-gap: 20px;
}

@media only screen and (max-width: 1190px) {
  .therapists {
    grid-template-columns: 1fr;
  }
}
.shrinkable-photo {
  max-width: 100%; 
  height: auto;
}
</style>
