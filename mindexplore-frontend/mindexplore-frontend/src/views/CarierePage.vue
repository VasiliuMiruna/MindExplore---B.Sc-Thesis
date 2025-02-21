<template>
    <div class="page">
      <PageTitleBar title="Cariere" />
      <div class="page-wrapper">
    
        <p class="home-subheading">
          Ești terapeut și ai vrea să ni te alăuri?
          Completează datele tale și te vom chema la un interviu.
        </p>
        <v-btn to="/registerTherapist">Vreau să mă înregistrez ca terapeut!</v-btn>
        <div class="image">
            <v-img
                
                :src="imagePath"
                style="max-width: 500px; max-height: 300px; margin: 0 auto;"
                ></v-img>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import PageTitleBar from '../components/PageTitleBar.vue';
  
  export default {
    components: {
      PageTitleBar
    },
    computed: {
        imagePath() {
      return require('@/assets/brigada.png');
    },
    } 
  };
  </script>
  
  <style scoped>
  .page {
    padding: 2rem;
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
  