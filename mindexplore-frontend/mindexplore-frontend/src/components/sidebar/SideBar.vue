<template>
  <div class="sidebar" :style="{ width: sidebarWidth, minWidth: sidebarWidth }">
    <h1 class="title">
      <span v-if="collapsed">
        <!-- <div>M</div>
        <div>E</div> -->
        <!-- <v-img
                
                :src="imagePath"
                style="max-width: 500px; max-height: 300px;"
          ></v-img> -->
          <p>M</p>
          <p>E</p>
      </span>
      <div v-else>
        <p>
          <router-link to="/">Mind Explore</router-link></p>
        <span class="loggedin-user" v-if="userStore.user"> {{ userStore.user.name }}</span>
      </div>
      
    </h1>
    <SideBarLink to="/home" icon="fas fa-home"> Acasă</SideBarLink>
    <div v-if="userStore.user.role !== 'Therapist'">
      <SideBarLink to="/therapists" icon="fas fa-users">Terapeuți</SideBarLink>
    </div>
    
     <div v-if="userStore.user.role !== 'Patient' && userStore.user.role !== 'Therapist'">
     <SideBarLink to="/cariere" icon="fas fa-handshake">Cariere</SideBarLink> 
    </div>
    <div v-if="userStore.user.role !== 'Patient' && userStore.user.role !== 'Therapist' && userStore.user.role !== 'Admin'">
     <SideBarLink to="/login" icon="fas fa-sign-in-alt">Autentificare</SideBarLink> 
    </div>
    
    
    <div v-if="userStore.user.role == 'Patient'">
    <SideBarLink v-if="userStore.user.userId" to="/appointments" icon="fas fa-calendar-days">Programări</SideBarLink>
    <SideBarLink v-if="userStore.user.userId" to="/conversations/:id" icon="fas fa-message">Conversații</SideBarLink>
    <SideBarLink v-if="userStore.user.userId" to="/personalTherapists" icon="fas fa-users">Terapeuții mei</SideBarLink>
    <SideBarLink v-if="userStore.user.userId" to="/notes" icon="fas fa-book">Jurnal</SideBarLink>
    
    </div>
    <div v-if="userStore.user.role == 'Admin'">
      <SideBarLink to="/admin/therapists" icon="fas fa-cog">Noi Terapeuți</SideBarLink>
      <!-- <SideBarLink to="/admin/patients" icon="fas fa-cog">Pacienți</SideBarLink> -->
    </div>
    <div v-if="userStore.user.role == 'Therapist'">
      <SideBarLink :to="therapistAppointmentsRoute" icon="fas fa-calendar-days" >Programări</SideBarLink>
      <SideBarLink to="/therapist/patients" icon="fas fa-users">Pacienți</SideBarLink>
      <SideBarLink to="/therapist/patients/:userId/conversation" icon="fas fa-message">Conversații</SideBarLink>
      
      
    </div>
    <div style="flex-grow: 1"></div>
    <button @click="$miscStore.switchTheme" class="theme-toggle">
      <span v-if="$miscStore.theme == 'light'"><i class="fas fa-moon"></i></span>
      <span v-if="$miscStore.theme == 'dark'"><i class="fas fa-sun"></i></span>
      
    </button>
    
    <button v-if="userStore.user.role === 'Patient' || userStore.user.role === 'Therapist' || userStore.user.role ==='Admin'" class="logout-button" @click="logout">
        <i class="fas fa-sign-out-alt"></i>
        <span v-if="!collapsed" class="logout-text">Deconectare</span>
    </button>
    
  
   
    <span class="collapse-icon" :class="{ 'rotate-180': collapsed }" @click="toggleSidebar">
      <i class="fas fa-angle-double-left"/>
    </span>
  </div>
</template>

<script setup>
import { computed } from 'vue';
import {collapsed, toggleSidebar, sidebarWidth} from './state'
import SideBarLink from './SideBarLink.vue';
import {useUserStore} from '@/store/user';
import {useMiscStore} from '@/store/misc';
import { useRouter } from 'vue-router'; 
const userStore = useUserStore();
const $miscStore = useMiscStore();
const router = useRouter();

const therapistAppointmentsRoute = computed(() => {
  return `/therapist/${userStore.user.tableUserId.toString()}/appointments`;
});



const logout = () => {
  console.log(localStorage.getItem('token'))
  localStorage.removeItem('userId');
  localStorage.removeItem('token');
  localStorage.removeItem('tableUserId');
  localStorage.removeItem('role');
  localStorage.removeItem('name');

  console.log(localStorage.getItem('token'))
  
  router.push('/home');
  window.location.reload();
};
// const isLoggedIn = computed(() => {
  
//   if ( userStore.token !== null)
//     return true;
//   else return false;
// });


</script>

<style scoped lang="scss">
.sidebar {
  display: flex;
  flex-direction: column;
  color: white;
  background-color: var(--sidebar-bg-color);
  z-index: 1;
  top: 0;
  left: 0;
  bottom: 0;
  padding: 0.5em;
  align-items: start;
  transition: 0.3s ease;

   .title {
     padding: 10px 0;
     margin: 0;
  //   color: #5720c6;
  
 
   }

  p {
    margin: 0;
    padding: 0;
   // font-family: 'Black Han Sans' Regular 400;
   
    //font-family: 'Yatra One', cursive;
   // font-family: 'Indie flower', Regular 400;

   font-family: 'Raleway', sans-serif;
    font-weight: 800;

  //font-family: 'Dancing Script', cursive;
   //font-weight: bold;
//   font-family: 'Anton', sans-serif;
// font-weight: 700;
//font-family: 'Kaushan Script', cursive;


    font-size: 36px;
    cursor: pointer;
//     background: #6F11B8;
    // background: linear-gradient(to right, #6F11B8 0%, #8F76C1 100%);
    // background: #5F11B8;
    // background: linear-gradient(to right, #5F11B8 0%, #8076C1 100%);
    background: #5C19CF;
    background: linear-gradient(to right, #5C19CF 0%, #6E8ECF 100%);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
        
  }
}


.collapse-icon {
  cursor: pointer;
  display: inline;
  // position: absolute;
  bottom: 0;
  padding: 1em 0.5em;
  color: rgba(255, 255, 255, 0.7);
  transition: 0.2s linear;
}

.rotate-180 {
  transform: rotate(180deg);
  transition: 0.2s linear;
}

.loggedin-user {
  font-size: 14px;
}

.theme-toggle {
  cursor: pointer;

  svg {
    transform: scale(1.3);
  }

  height: 32px;
  width: 32px;
  background: #ccc;
  border-radius: 50%;
  border: none;
}

.logout-button {
  margin: 5px;
  display: flex;
  align-items: center;
  padding: 0.25em 0.5em; /* Adjust the padding values as needed */
  background-color: transparent;
  border: none;
  color: white;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.3s;

  &:hover {
    background-color: rgba(255, 255, 255, 0.1);
  }

}
</style>
