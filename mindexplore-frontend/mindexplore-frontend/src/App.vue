<template>
  <div id="app" :class="theme">
    <SideBar v-if="showSidebar" />
    <div class="router-view">
      <router-view />
    </div>
  </div>
</template>

<script>
import { mapState } from 'pinia';
import SideBar from './components/sidebar/SideBar.vue'
import { useMiscStore } from './store/misc';

export default {
  components: { SideBar },
  data: () => ({
  }),
  computed: {
    ...mapState(useMiscStore, ['theme']),
    showSidebar() {
      return !["Login", "Register", "RegisterTherapist"].includes(this.$route.name)
    }
  },
  methods: {
  },
  mounted() {
    console.log(this.theme)
  }
}
</script>

<style lang="scss">
@import "./assets/global.scss";

#app {
  min-height: 100vh;
  min-width: 100vw;
  max-height: 100vh;
  display: flex;
  flex-direction: row;

  .sidebar {
    // height: 100%;
  }

  .router-view {
    flex-grow: 1;
    // padding: 20px;
  }
}
</style>



