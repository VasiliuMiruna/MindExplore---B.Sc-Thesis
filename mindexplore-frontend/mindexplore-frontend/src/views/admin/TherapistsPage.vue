<template>
    <div class="page">
      <PageTitleBar title="Terapeuți noi"/>
      <div class = "page-wrapper" >
        
          <section class="therapists">
          <NewTherapistCard v-for="therapist in therapists" :key="therapist.id" :therapistCard="therapist">
            {{ therapist.firstName }}
          </NewTherapistCard>
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
import NewTherapistCard from '@/components/NewTherapistCard.vue';
export default {
  components: {
    PageTitleBar,
    NewTherapistCard
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
        const response = await axios.get(`${API_BASE}/api/Therapist/notAdded`);
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
@media only screen and (max-width: 800px) {
  .therapists {
    grid-template-columns: 1fr;
  }
}
</style>