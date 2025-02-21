<template>
  <div class="page">
    <PageTitleBar title="Programările mele" />
    <div class="page-wrapper">
      <WeekCalendar></WeekCalendar>
      <v-dialog v-model="dialog" persistent width="auto">
        <template v-slot:activator="{ props }">
          <v-btn color="primary" icon="fas fa-info" v-bind="props">

          </v-btn>

        </template>
        <v-card>
          <v-card-title class="text-h5">
            Cum funcționează calendarul?
          </v-card-title>
          <v-card-text>
            Programările care nu au fost încă acceptate de către terapeut sunt marcate cu
            <span style="color: orange; font-weight: bold;">portocaliu</span>, iar cele acceptate cu
            <span style="color: green; font-weight: bold;">verde</span>.

          </v-card-text>
          <v-card-text>
            Dă click pe un interval pentru a vedea mai multe detalii.

          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="green-darken-1" variant="text" @click="dialog = false">
              Inchide
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </div>

  </div>
</template>
<script>
import WeekCalendar from '@/components/WeekCalendar.vue';
import PageTitleBar from '../components/PageTitleBar.vue';
export default {
  data() {
    return {
      dialog: false,
    }
  },
  components: {
    PageTitleBar,
    WeekCalendar
  },

}
</script>

<style scoped lang="scss">
@media (max-width: 768px) {

  /* Styles for small screens */
  .page-wrapper {
    flex-direction: column !important;
  }
}

@media (min-width: 769px) and (max-width: 1024px) {

  /* Styles for medium screens */
  .page-wrapper {
    flex-direction: row;
    justify-content: space-between;
  }
}

@media (min-width: 1025px) {

  /* Styles for large screens */
  .page-wrapper {
    flex-direction: row;
    justify-content: space-around;
  }
}
</style>