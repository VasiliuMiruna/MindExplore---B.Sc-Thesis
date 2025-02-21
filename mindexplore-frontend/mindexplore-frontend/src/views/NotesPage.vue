<template>
  <div class="page">
    <PageTitleBar title="Jurnal"/>
    <div class="page-wrapper pt-4" style="flex-direction: column;">
      <div class="notes-page">
        <div>
        <div>
          <v-card class="pa-2">
            <v-textarea label="Cum te simți azi?" variant="outlined" v-model="newNote"></v-textarea>
            <p class="text-red" v-if="errorMessage">{{ errorMessage }}</p>
            <v-btn
            class="ma-2"
            color="#088cfc"
            icon="fas fa-regular fa-face-smile-beam"
            :class="{ active: selectedFeeling === 'happy' }"
            @click="toggleFeeling('happy')"
          ></v-btn>
          <v-btn
            class="ma-2"
            color="#08e494"
            icon="fas fa-regular fa-face-meh"
            :class="{ active: selectedFeeling === 'neutral' }"
             @click="toggleFeeling('neutral')"
          ></v-btn>
          <v-btn
            class="ma-2"
            color="#ffb41c"
            icon="fas fa-regular fa-face-frown"
            :class="{ active: selectedFeeling === 'sad' }"
             @click="toggleFeeling('sad')"
          ></v-btn>
            <v-btn size="small" @click="newNote != '' && createNote()">save note</v-btn>
          </v-card>
        </div>
        <div id="chart">
          <h2>In ultimele 7 zile:</h2>
          <apexchart type="pie" width="380" :options="chartOptions" :series="series"></apexchart>
          
        </div>
        <!-- <div class="chart-container">
              <v-dialog v-model="chartDialog" max-width="600">
                <template #activator="{ on }">
                  <v-btn color="primary" v-on="on">Open Chart</v-btn>
                </template>
                <v-card>
                  <div class="chart-inner-container">
                    <h2>In ultimele 7 zile:</h2>
                    <apexchart type="pie" width="380" :options="chartOptions" :series="series"></apexchart>
                  </div>
                </v-card>
              </v-dialog>
</div> -->
        </div>
        <div class="notes-list">
          <v-card v-for="note in notes" :key="note.id" class="mb-5">
            <v-card-title class="font-weight-bold">{{ getFormattedDate(note.createdDate) }}</v-card-title>
            <v-card-text v-if="note.showContent">{{ note.content }}</v-card-text>
            <!-- <v-card-text >{{ note.content }}</v-card-text> -->
            <v-btn
              v-if="!note.showContent"
              @click="toggleNoteContent(note.id)"
             
            >
              Deschide
            </v-btn>
            <v-btn
              v-if="note.showContent"
              @click="toggleNoteContent(note.id)"
              
            >
              Închide
            </v-btn>
            <v-btn size="x-small" class= "delete-button" color="red" icon="mdi-delete-outline" @click="deleteNote(note.id)"></v-btn>
          </v-card>
        </div>
        
        
      </div>

    </div>
  </div>
</template>

<script>
import PageTitleBar from "@/components/PageTitleBar.vue";
import axios from "axios";
import {API_BASE} from "@/services/constants";
import {mapState} from "pinia";
import {useUserStore} from "@/store/user";
import VueApexCharts from 'vue3-apexcharts';
export default {
  components: {
    PageTitleBar,
    apexchart: VueApexCharts,
  },
  data() {
    return { 
      //for the chart
      chartDialog: false,
      series: [1, 2, 4],
      chartOptions: {
        chart: {
          width: 380,
          type: 'pie',
        },
        labels: ['Fericit', 'Neutru', 'Trist'],
        responsive: [{
          breakpoint: 480,
          options: {
            chart: {
              width: 200
            },
            legend: {
              position: 'bottom'
            }
          }
        }]
      },
      //end of for the chart

      showModal: false,
      newNote: "",
      errorMessage: "",
      notes: [],
      selectedFeeling: null,
      feelingStatistics: {
          happy: 0,
          neutral: 0,
          sad: 0
     },
    };
  },
  
  async created() {
    this.fetchNotes();
  },
  methods: {
    toggleFeeling(feeling) {
    if (this.selectedFeeling === feeling) {
      this.selectedFeeling = null; // Deselect the feeling if already selected
    } else {
      this.selectedFeeling = feeling; // Select the feeling if not selected
    }
  },
    getFormattedDate(dateString) {
      return new Date(dateString).toLocaleDateString("en-GB")
    },
    getRandomColor() {
      return "hsl(" + Math.random() * 360 + ", 100%, 75%)";
    },
    async fetchNotes() {
      try {
        const response = await axios.get(
            `${API_BASE}/api/Patients/${this.user.userId}/notes`,
            {
            
          }
        );
        this.notes = response.data.reverse();
        const response2 = await axios.get(`${API_BASE}/api/Patients/${this.user.tableUserId}/getFeelingStatistic`,
        {
          headers: {
              'Authorization': 'Bearer ' + this.token,
              Role: 'Patient',
            },
          });
          this.feelingStatistics.happy = response2.data.happy;
          this.feelingStatistics.neutral = response2.data.neutral;
          this.feelingStatistics.sad = response2.data.sad;
          this.series = [
                this.feelingStatistics.happy,
                this.feelingStatistics.neutral,
                this.feelingStatistics.sad
         ];
        console.log(response.data);
      } catch (error) {
        console.error(error);
      }
    },
    async createNote() {
     

      // const newNote = {
      //   content: this.newNote,
      // };
      if (!this.selectedFeeling) {
        return (this.errorMessage = "Please select a feeling!");
      }

      const newNote = {
        content: this.newNote,
        feeling: this.selectedFeeling, // Add this line
      };

      try {
        await axios.post(
            `${API_BASE}/api/Patients/${this.user.tableUserId}/notes`,
            newNote
        );
        this.fetchNotes()
        this.newNote = "";
        this.selectedFeeling = null;
        this.errorMessage = "";
      } catch (error) {
        console.error(error);
      }
    },
    async deleteNote (noteId) {
      try {
        console.log(noteId)
        await axios.delete(`${API_BASE}/api/Patients/${this.user.tableUserId}/notes/${noteId}`,
        {
          headers: {
              'Authorization': 'Bearer ' + this.token,
              Role: 'Patient',
            },}
        );
        this.fetchNotes();
      }catch(error) {
        console.error(error)
      }
    },
    toggleNoteContent(noteId) {
    const note = this.notes.find((n) => n.id === noteId);
    if (note) {
      note.showContent = !note.showContent;
    }
  },
  },
  computed: {
    ...mapState(useUserStore, ['user']),
    token() {
      return localStorage.getItem("token");
    },
  }
};
</script>

<style lang="scss" scoped setup>
.notes-page {
  display: grid;
  grid-template-columns: auto 1fr 1fr;
  grid-column-gap: 20px;
  grid-row-gap: 20px;
}
.delete-button {
  position: absolute;
  top: 10px;
  right: 10px;
  z-index: 9999;
}
.chart-container {
 
  grid-column: 3;
}
.notes-list {
  grid-column: span 2;
}
#chart {
  margin-top: 30px;
}
.chart-container {
  padding: 16px;
}
@media (max-width: 960px) {
  .page-wrapper {
    flex-direction: column;
  }
}
</style>
