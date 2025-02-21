<template>
    <div class="page">
      <PageTitleBar title="Terapeuții mei"/>
      <div class = "page-wrapper" >
        
          <section class="therapists">
          <TherapistPersonalCard v-for="therapist in therapists" :key="therapist.id" :therapistCard="therapist">
            {{ therapist.firstName }}
          </TherapistPersonalCard>
          </section>
        </div>
      </div>
  
</template>

<script>
import axios from 'axios';
import {API_BASE} from '@/services/constants';
import PageTitleBar from '@/components/PageTitleBar.vue';
import {mapState} from "pinia";
import {useUserStore} from "@/store/user"; 
import TherapistPersonalCard from '@/components/TherapistPersonalCard.vue';
export default {
  components: {
    PageTitleBar,
    TherapistPersonalCard
},
  data() {
    return {
      therapists: []
    }
  },
  async mounted() {
    this.fetchTherapists()
  },
  methods: {
    async fetchTherapists () {
      try {
        const response = await axios.get(`${API_BASE}/api/Patients/${this.user.tableUserId}/therapists`);
        this.therapists = response.data;
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

.therapists {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  grid-column-gap: 20px;
  grid-row-gap: 20px;

}
.v-card {
    max-height: 300px;
}
@media only screen and (max-width: 1190px) {
  .therapists {
    grid-template-columns: 1fr;
  }
}
</style>