<template>
    <div class="page">
        <PageTitleBar title="Cei mai potriviti doi terapeuți:"/>
        <div class="page-wrapper">
           <section class="therapists">
            <TherapistCard v-for="therapist in therapists" :key="therapist.id" :therapistCard="therapist">
            </TherapistCard>
        </section>
        </div>
        
    </div>
</template>
<script>
import PageTitleBar from '../components/PageTitleBar.vue';
import { API_BASE } from '@/services/constants';
import {mapState} from "pinia";
import {useUserStore} from "@/store/user";
import axios from 'axios';
import TherapistCard from '../components/TherapistCard.vue';
export default {

    data() {
    return {
        therapists:[]
    }
    },
    components: {
        PageTitleBar,
        TherapistCard
    },
    mounted() {
    this.fetchReccomendations();
    
    },
    computed: {
        ...mapState(useUserStore, ['user']),
    token() {
      return localStorage.getItem("token");
    },
  },
  methods: {
    async fetchReccomendations()
    {
        try {
            const response = await axios.get(`${API_BASE}/api/Test/${this.user.tableUserId}/recommendations`);
            console.log(response.data);
            this.therapists = response.data;
        } catch (error) {
            console.error(error)
        }
        
    }
  }
}
</script>
<style lang="scss" scoped>
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