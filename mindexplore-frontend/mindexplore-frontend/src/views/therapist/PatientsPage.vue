<template>
    <div class="page">
      <PageTitleBar title="Pacienții mei"/>
      <div class = "page-wrapper" style="flex-direction: column;">
        <div class="patients">
          <PatientCard v-for="patient in patients" :key="patient.id" :patientCard="patient">
            
            {{ patient.firstName }}
          </PatientCard>

        </div>
      </div>
    </div>
</template>

<script>
import axios from 'axios';
import {API_BASE} from '@/services/constants';
import PageTitleBar from '@/components/PageTitleBar.vue';
import {mapState} from "pinia";
import {useUserStore} from "@/store/user"; 
import PatientCard from '@/components/PatientCard.vue';
export default {
  components: {
    PageTitleBar,
    PatientCard
},
  data() {
    return {
      patients: []
    }
  },
  async mounted() {
    this.fetchPatients()
  },
  methods: {
    async fetchPatients () {
      try {
        const response = await axios.get(`${API_BASE}/api/Therapist/${this.user.tableUserId}/patients`);
        this.patients = response.data;
        console.log(response.data)
        
      } catch (error) {
        console.error(error);
      }
    }
  },
   computed: {
    ...mapState(useUserStore, ['user']),
  },
}
</script>
<style scoped>

.patients {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  grid-column-gap: 20px;
  grid-row-gap: 20px;

}
.v-card {
    max-height: 300px;
    max-width: 796px
}
@media only screen and (max-width: 1190px) {
  .patients {
    grid-template-columns: 1fr;
  }
}
</style>