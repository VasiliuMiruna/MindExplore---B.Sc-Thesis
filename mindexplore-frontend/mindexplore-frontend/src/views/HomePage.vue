<!-- <template>
    <div class="page">
        <PageTitleBar title="Acasă"/>
        <div class="page-wrapper">
            <v-btn to="/test">
                Gaseste un terapeut
            </v-btn>
        </div>
        
    </div>
</template>
<script>
import PageTitleBar from '../components/PageTitleBar.vue';
export default {
    components: {
        PageTitleBar
    }
}
</script> -->
<template>
    <div class="page">
      <PageTitleBar title="Acasă" />
      <div  class="page-wrapper">
        <div v-if="this.user.role === 'Therapist'" >
          <p class="home-subheading">
            Bine ai venit!
            Adaugă datele la care ești disponibil și vei putea vizualiza detalii despre pacienții care vor să se conecteze cu tine!
          </p>
          <div class="image">
              <v-img
                  
                  :src="imagePathTherapist2"
                  style="max-width: 500px; max-height: 300px; margin: 0 auto;"
                  ></v-img>
          </div>
          <p class="home-subheading">
            Ține legătura cu ei și între ședințe, pornind o conversație cu ei! Nu uita 
            <span style="font-weight: bold;">doar tu</span>
            poți începe o conversație cu un pacient, acesta nu are această posibilitate
          </p>
          <div class="image">
              <v-img
                  
                  :src="imagePathTherapist"
                  style="max-width: 500px; max-height: 300px; margin: 0 auto;"
                  ></v-img>
          </div>
          <p class="home-subheading">
            Nu uita să îți completezi profilul! Pacieților le va fi mai ușor să te cunoască!
          </p>
          
          <div class="image">
              <v-img
                  
                  :src="imagePathTherapist3"
                  style="max-width: 500px; max-height: 300px; margin: 0 auto;"
                  ></v-img>
          </div>
          <p class="home-subheading">
             Mai ai vreo nedumerire? Lasă-ne un email la adresa 
             <span style="font-weight: bold;">mindexploretest@gmail.com</span> și te va contacta cineva din echipa noastră.
          </p>
        </div>
        <div v-else>
          <div class="test">
              <p class="home-subheading">
              Aici poți găsi un terapeut potrivit pentru nevoile tale și începe o călătorie către îmbunătățirea
              stării tale mentale și emoționale.</p>
              <v-btn to="/test">Găsește un terapeut</v-btn>
              
          </div>
          <div class="image">
              <v-img
                  
                  :src="imagePath"
                  style="max-width: 500px; max-height: 300px; margin: 0 auto;"
                  ></v-img>
          </div>
          <p class="home-subheading">
              Tot ce trebuie să faci este să te înscrii, să completezi formularul, iar noi îți vom recomanda cei mai 
              potriviți doi terapeuți pentru tine. 
              Toți terapeuții de pe platforma noastră sunt acreditați.
              Nu îți dorești o recomandare? Poți vizualiza toți terapeuții noștri și face o alegere de unul singur!
              </p>
        <!-- <p>
           Nu îți dorești o recomandare? Poți vizualiza toți terapeuții noștri și face o alegere de unul singur!
        </p> -->
        <div class="image">
              <v-img
                  
                  :src="imagePath2"
                  style="max-width: 500px; max-height: 300px; margin: 0 auto;"
                  ></v-img>
          </div>
        <p class="home-subheading">
             Mai ai vreo nedumerire? Lasă-ne un email la adresa 
             <span style="font-weight: bold;">mindexploretest@gmail.com</span> și te va contacta cineva din echipa noastră.
          </p>
     
       </div>
 
        
      </div>
      

      </div>
    
  </template>
  
  <script>
  import PageTitleBar from '../components/PageTitleBar.vue';
  import {mapState} from "pinia";
  import {useUserStore} from "@/store/user";
  export default {
    components: {
      PageTitleBar
    },
    computed: {
      ...mapState(useUserStore, ['user']),
        imagePath() {
      return require('@/assets/Asset2.png');
      },
      imagePath2(){
      return require('@/assets/Asset6.png');
      },

      imagePathTherapist() {
        return require('@/assets/calendar (1).png');
      },
      imagePathTherapist2() {
        return require('@/assets/Asset4.png');
      },
      imagePathTherapist3() {
        return require('@/assets/heart.png');
      }
    },
    
  };
  </script>
  
  <style scoped>
  .page {
    padding: 2rem;
  }
  .test {
    
  }
  .page-wrapper {
    max-width: 800px;
    margin: 0 auto;
    text-align: center;
    flex-direction: column;
  }
  
  .home-heading {
    font-size: 32px;
    margin-bottom: 1.5rem;
  }
  
  .home-subheading {
    font-size: 18px;
    color: #666;
    margin-bottom: 2rem;
  }
  
  </style>
  