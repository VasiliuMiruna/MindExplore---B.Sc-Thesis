<template>
  <div class="page">
    <div>
      Sloturi disponibile:
    </div>
    <div v-for="slot in slots" :key="slot.id">
      {{ formatDate(slot.startTime) }} - {{ formatDate2(slot.endTime) }}
      <v-btn v-if ="isPatient" label="Programeaza-te" @click="createAppointment(slot)">
        Programeaza-te
      </v-btn>
      
      <v-btn v-if="isTherapist" label="Sterge" @click="deleteSlot(slot)">
        Sterge slot
      </v-btn>
    </div>

    <v-btn density="compact" icon="fa-plus" @click="showDatePicker = true">+</v-btn>
    <v-dialog v-model="showDatePicker">
      <v-card>
        <v-card-title>
          Alege data
        </v-card-title>
        <v-card-text>
          <input type="date" v-model="selectedDate">
          <input type="time" v-model="selectedStartTime">
          <input type="time" v-model="selectedEndTime">
          <v-date-picker v-model="selectedDate" no-title @input="showTimePicker = true"></v-date-picker>
          <v-time-picker v-if="showTimePicker" v-model="selectedStartTime" @input="showEndTimePicker = true"></v-time-picker>
          <v-time-picker v-if="showEndTimePicker" v-model="selectedEndTime" @input="createSlot"></v-time-picker>
        </v-card-text>
        <v-card-actions>
          <v-btn color="primary" @click="showDatePicker = false; showTimePicker = false; showEndTimePicker = false">Anuleaza</v-btn>
          <v-btn @click="createSlot">Creeaza slot</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

  </div>
</template>

<script>
import axios from 'axios';
import { API_BASE } from '@/services/constants';
import { VBtn, VDialog, VCard, VCardTitle, VCardText, VCardActions, VDatePicker, VTimePicker } from 'vuetify/components';
import {mapState} from "pinia";
import {useUserStore} from "@/store/user";
export default {
  name: 'SlotCalendar',
  components: {
    VBtn,
    VDialog,
    VCard,
    VCardTitle,
    VCardText,
    VCardActions,
    VDatePicker,
    VTimePicker,
  },
 
  data() {
    return {
      appointmentModel: {
        id: null,
        start: null,
        end: null,
        description: null,
      },
      slotModel: {
        id: null,
        startTime: null,
        endTime: null,
        available: true,
      },
      slots: [],
      showDatePicker: false,
      showTimePicker: false,
      showEndTimePicker: false,
      selectedDate: null,
      selectedStartTime: null,
      selectedEndTime: null,
    };
  },
  computed: {
    ...mapState(useUserStore, ['user']),
    token() {
      return localStorage.getItem("token");
    },
    isPatient() {
      return this.user.role === "Patient"
    },
    isTherapist() {
    return this.user.role === 'Therapist';
  },
  },
  async mounted() {
    try {
      this.tableTherapistId = parseInt(this.$route.params.tableUserId);
      console.log(this.tableUserId);
      console.log(this.tableUserId)
      
      const response = await axios.get(`${API_BASE}/api/therapist/${this.tableTherapistId}/slots`,{
      headers: {
        'Authorization': 'Bearer ' + this.token,
        'Role': 'Therapist' 
       }
      });
      this.slots = response.data;
      console.log(response.data);
    } catch (error) {
      console.error(error);
    }
  },
  methods: {
    async createSlot() {
      const startDateTime = new Date(this.selectedDate);
      const [startHours, startMinutes] = this.selectedStartTime.split(':');
      startDateTime.setHours(startHours, startMinutes);
      const endDateTime = new Date(this.selectedDate);
      const [endHours, endMinutes] = this.selectedEndTime.split(':');
      endDateTime.setHours(endHours, endMinutes);
      const formattedStart = startDateTime.toISOString();
      const formattedEnd = endDateTime.toISOString();
      const newSlot = {
        startTime: formattedStart,
        endTime: formattedEnd,
        available: true,
      };
      try {
        const response = await axios.post(`${API_BASE}/api/therapist/${this.tableTherapistId}/slots`, newSlot,  {
          headers: {
            'Authorization': 'Bearer ' + this.token,
            'Role': 'Therapist'
          }
        });
        this.slots.push(response.data);
        console.log(response.data);
      } catch (error) {
        console.error(error);
      }
    },
    async deleteSlot(slot) {
      try {
        await axios.delete(`${API_BASE}/api/therapist/${this.tableTherapistId}/${slot.id}/slots`, {
          headers: {
            'Authorization': 'Bearer ' + this.token,
            'Role': 'Therapist'
          }
        });
        // Remove the slot from the local array
        if (this.selectedSlotId === slot.id) {
          this.selectedSlotId = null;
        }
        const index = this.slots.findIndex(s => s.id === slot.id);
        if (index !== -1) {
          this.slots.splice(index, 1);
        }
      } catch (error) {
        console.error(error);
      }
   
    },
    async createAppointment(slot) {
      const startDateTime = new Date(slot.startTime);
      const endDateTime = new Date(slot.endTime);
      const formattedStart = startDateTime.toISOString();
      const formattedEnd = endDateTime.toISOString();
      const newAppointment = {
        startTime: formattedStart,
        endTime: formattedEnd,
        slotId: slot.id,
      };
      try {
        const response = await axios.post(`${API_BASE}/api/Patients/1/appointments`, newAppointment);
        this.slots.push(response.data);
      } catch (error) {
        console.error(error);
      }
    },
    formatDate(date) {
      const options = {
        weekday: 'long',
        month: 'numeric',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
        hour12: false,
      };
      const romaniaLocale = 'ro-RO';
      const formattedDate = new Date(date).toLocaleString(romaniaLocale, options);
      return formattedDate;
    },
    formatDate2(date) {
      const options2 = {
        hour: 'numeric',
        minute: 'numeric',
        hour12: false,
      };
      const romaniaLocale = 'ro-RO';
      const formattedEndDate = new Date(date).toLocaleString(romaniaLocale, options2);
      return formattedEndDate;
    },
  },
};
</script>
