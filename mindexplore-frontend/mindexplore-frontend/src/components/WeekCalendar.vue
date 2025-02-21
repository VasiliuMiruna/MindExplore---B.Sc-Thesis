<template>
  <div class="wrap">
    <div class="left">
      <DayPilotNavigator id="nav" :config="navigatorConfig" />
    </div>
    <div class="content">
      <DayPilotCalendar id="dp" :config="config" ref="calendar" />
    </div>


    <v-dialog v-model="dialogVisible" max-width="500px">
      <v-card>
        <v-card-title>Detalii pacient</v-card-title>
        <v-card-text>
          <!-- <p>Ora de inceput: {{ this.appointmentModel.startTime }}</p>
          <p>Ora de final: {{ this.appointmentModel.endTime }}</p> -->
          <p>Nume pacient: {{ this.patientFirstName }} {{ this.patientLastName }}</p>
          <p v-if="this.patientAge !== null">Varsta: {{ this.patientAge }}</p>

          <router-link
            :to="{ path: '/' + this.appointmentModel.patientId + '/test', params: { id: this.appointmentModel.patientId } }">
            <v-btn color="primary" variant="tonal" prepend-icon="mdi-book" class="test-button">Chestionar


            </v-btn>
          </router-link>
        </v-card-text>

        <v-card-actions>
          <v-btn v-if="this.pending === true" color="primary"
            @click="acceptAppointment(this.appointmentModel)">Accepta</v-btn>
          <v-btn v-if="this.pending === true" color="primary"
            @click="refuseAppointment(this.appointmentModel)">Refuza</v-btn>
          <v-btn color="secondary" @click="this.dialogVisible=false">Inchide</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="dialogTherapistsVisible" max-width="500px">
      <v-card>
        <v-card-title>Nume terapeut</v-card-title>
        <v-card-text>
          <!-- <p>Ora de inceput: {{ this.appointmentModel.startTime }}</p>
          <p>Ora de final: {{ this.appointmentModel.endTime }}</p> -->
          <p> {{ this.therapistFirstName }} {{ this.therapistLastName }}</p>
          <p v-if="this.appointmentModel.status === 'Pending'">Programarea nu a fost acceptata inca</p>
        </v-card-text>

        <v-card-actions>
          <v-btn color="secondary" @click="dialogTherapistsVisible=false">Inchide</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="dialogPatientVisible" max-width="500px">
      <v-card>
        <v-card-title>Doresti o programare?</v-card-title>
        <v-card-actions>
          <v-btn v-if="this.pending === true" color="primary" @click="createAppointment(this.slotModel)">Da</v-btn>

          <v-btn color="secondary" @click="dialogPatientVisible=false">Inchide</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="dialogDeleteSlot" max-width="500px">
      <v-card>
        <v-card-title>Doresti sa stergi acest slot?</v-card-title>
        <v-card-actions>
          <v-btn color="red" @click="deleteSlot(this.slotModel)">Da</v-btn>

          <v-btn color="secondary" @click="dialogDeleteSlot=false">Inchide</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { DayPilot, DayPilotCalendar, DayPilotNavigator } from '@daypilot/daypilot-lite-vue';
import axios from 'axios';
import { API_BASE } from '@/services/constants';
import { mapState } from "pinia";
import { useUserStore } from "@/store/user";

export default {
  name: 'WeekCalendar',
  data() {
    return {
      closeSlotModel: false,
      pending: true,
      therapistId: null,
      events: [],
      dialogVisible: false,
      patientFirstName: null,
      patientLastName: null,
      patientAge: null,
      therapisttFirstName: null,
      therapistLastName: null,
      dialogTherapistsVisible: false,
      dialogPatientVisible: false,
      dialogDeleteSlot: false,
      slotModel: {
        id: null,
        startTime: null,
        endTime: null,
        available: true,
      },
      appointmentModel: {
        id: null,
        startTime: null,
        endTime: null,
        location: null,
        status: null,
        patientId: null,
        therapistId: null,
        slotId: null,
      },
      navigatorConfig: {
        showMonths: 1,
        skipMonths: 1,
        selectMode: 'Week',
        locale: 'ro-RO',
        timezone: 'Europe/Bucharest',
        startDate: '2023-06-01',
        selectionDay: '2023-06-28',
        onTimeRangeSelected: (args) => {
          this.config.startDate = args.day;
        },

      },
      config: {
        eventBackColor: 'orange',
        viewType: 'Week',
        locale: 'ro-RO',
        headerDateFormat: 'dddd dd',
        timezone: 'Europe/Bucharest',
        cellDuration: 60,
        businessBeginsHour: 8,
        businessEndsHour: 20,
        durationBarVisible: false,
        timeRangeSelectedHandling: 'Hold',
        eventClickHandling: 'Enabled', // Enable event click handling

        onTimeRangeSelected: async (args) => {
          console.log("argumente")
          console.log(args)
          if (this.user.role === 'Therapist') {
            const modal = await DayPilot.Modal.prompt('Alege un interval potrivit pentru tine:', 'Liber');
            const dp = args.control;

            // const dpStart = args.start.toISOString();
            // const dpStart = new Date(args.start).toISOString();
            // const dpEnd = new Date(args.end).toISOString();
            const dpStart = new Date(args.start);
            const dpEnd = new Date(args.end);

            console.log("!!!!!!!!!")
            if (modal.canceled) {
              dp.clearSelection();
              return;
            }
            console.log("sksksksk")
            const selectedRanges = Array.from(dp.getSelection());
            console.log(selectedRanges)
            console.log(dp.getSelection())
            console.log("start")
            console.log(dpStart)
            console.log(dpEnd)
            this.createSlot(dpStart, dpEnd);
          }


        },


        onEventClick: async (args) => {
          const clickedEvent = args.e.data;
          console.log(clickedEvent)
          if (clickedEvent.backColor === 'orange' && this.user.role === 'Therapist') {

            this.slotModel = args.e.data;
            console.log("delete")
            this.dialogDeleteSlot = true;

          }
          else if (clickedEvent.backColor === 'orange' && this.$route.path === `/therapist/${this.therapistId}/appointments`) {
            if (this.user.role === 'Patient') {
              //const modal = await DayPilot.Modal.prompt('Dorești o programare?', 'Confirm');

              // if (!modal.canceled && modal.result === 'Confirm') {
              //   this.createAppointment(clickedEvent);
              // }
              this.slotModel = args.e.data;
              this.dialogPatientVisible = true
            }
          }
          else if ((clickedEvent.backColor === 'orange' && this.user.role === 'Patient') || (clickedEvent.backColor === 'green' && this.user.role === 'Patient')) {

            this.appointmentModel = args.e.data;
            await this.fetchTherapistDetails(this.appointmentModel.therapistId);
            this.dialogTherapistsVisible = true;

          }
          else if (clickedEvent.backColor === 'red') {

            if (this.user.role === 'Therapist') {

              this.appointmentModel = args.e.data;
              await this.fetchPatientDetails(this.appointmentModel.patientId);
              this.pending = true;
              this.dialogVisible = true;

            }
          }

          else if (clickedEvent.backColor === 'green') {
            if (this.user.role === 'Therapist') {

              this.appointmentModel = args.e.data;
              await this.fetchPatientDetails(this.appointmentModel.patientId);
              this.pending = false;
              this.dialogVisible = true;

            }
          }

        },
        eventDeleteHandling: 'Disabled',
        onEventMoved: () => {
          console.log('Event moved');
        },
        onEventResized: () => {
          console.log('Event resized');
        },
      },
    };
  },
  components: {
    DayPilotCalendar,
    DayPilotNavigator,
  },
  computed: {
    calendar() {
      return this.$refs.calendar.control;
    },
    ...mapState(useUserStore, ['user']),
    token() {
      return localStorage.getItem("token");
    },
  },


  methods: {
    closeDialog() {
      this.dialogVisible = false;
    },
    async createSlot(start, end) {
      try {
        console.log(start)
        console.log(end)
        const newSlot = {
          // startTime: start,
          // endTime: end,
          startTime: new Date(start.getTime() + 3 * 60 * 60 * 1000), // Add 3 hours to the start time
          endTime: new Date(end.getTime() + 3 * 60 * 60 * 1000),
          available: true,
        };
        console.log(Intl.DateTimeFormat().resolvedOptions().timeZone)
        const response = await axios.post(
          `${API_BASE}/api/therapist/${this.therapistId}/slots`,
          newSlot,
          {
            headers: {
              'Authorization': 'Bearer ' + this.token,
              Role: 'Therapist',
            },
          }
        );

        if (response.status === 200) {
          // Slot created successfully
          const createdSlot = response.data;
          const event = new DayPilot.Event({
            id: createdSlot.id,
            start: this.startTime,
            end: this.endTime,
          });
          location.reload();
          this.calendar.events.add(event);

        } else {
          // Handle error
          console.error('Error creating slot:', response);
        }
      } catch (error) {
        // Handle error
        console.error('Error creating slot:', error);
      }
    },

    async createAppointment(clickedEvent) {
      try {
        const start = new Date(clickedEvent.start);
        start.setHours(start.getHours() + 3); // Add 3 hours to the current start time

        const end = new Date(start);
        end.setHours(end.getHours() + 1);
        const appointment = {
          startTime: start,
          endTime: end,
          therapistId: this.therapistId,
          patientId: this.user.tableUserId,
          slotId: clickedEvent.id,
          status: "Pending"
        };

        const response = await axios.post(
          `${API_BASE}/api/Patients/${this.user.tableUserId}/appointments`,
          appointment,
          {
            headers: {
              'Authorization': 'Bearer ' + this.token,
              Role: 'Patient',
            },
          }
        );

        if (response.status === 200) {
          // Appointment created successfully
          console.log("letsgo")
          // location.reload();
        } else {
          // Handle error
          console.error('Error creating appointment:', response);
        }

      } catch (error) {
        // Handle error
        console.error('Error creating appointment:', error);
      }
      location.reload();
    },
    async fetchPatientDetails(patientId) {
      const response = await axios.get(`${API_BASE}/api/Patients/${patientId}`, {
        headers: {
          'Authorization': 'Bearer ' + this.token,
          'Role': 'Patient'
        }
      })
      this.patientFirstName = response.data.firstName;
      this.patientLastName = response.data.lastName;
      this.patientAge = response.data.age;
    },
    async fetchTherapistDetails(therapistId) {
      const response = await axios.get(`${API_BASE}/api/Therapist/${therapistId}`)
      this.therapistFirstName = response.data.firstName;
      this.therapistLastName = response.data.lastName;
    },
    async acceptAppointment(clickedEvent) {
      try {
        const appointmentId = clickedEvent.id;
        const status1 = "Accepted";
        clickedEvent.backColor = 'green';
        const response = await axios.put(
          `${API_BASE}/api/Therapist/${this.therapistId}/appointments/${appointmentId}`,
          status1,
          {
            headers: {
              'Authorization': 'Bearer ' + this.token,
              'Content-Type': 'application/json', // Set the content type to JSON
              'Accept': 'application/json' // Add the Accept header
            },
          }
        );

        if (response.status === 200) {
          // Appointment accepted successfully
          clickedEvent.backColor = 'green'; // Update the event color to green
          location.reload();
          console.log("Accepted successfully");
        } else {
          // Handle error
          console.error('Error accepting appointment:', response);
        }
      } catch (error) {
        // Handle error
        console.error('Error accepting appointment:', error);
      }
    },
    async refuseAppointment(clickedEvent) {
      const appointmentId = clickedEvent.id;
      const status1 = "Rejected";
      const response = await axios.put(
        `${API_BASE}/api/Therapist/${this.therapistId}/appointments/${appointmentId}`,
        status1,
        {
          headers: {
            'Authorization': 'Bearer ' + this.token,
            'Content-Type': 'application/json', // Set the content type to JSON

          },
        }
      );
      if (response.status === 200) {
        // Appointment accepted successfully
        clickedEvent.backColor = 'yellow'; // Update the event color to green
        console.log("Rejected successfully");
        location.reload();
      }
    },
    async deleteSlot(slot) {
      const slotId = slot.id;
      console.log(slotId)
      await axios.delete(`${API_BASE}/api/Therapist/${this.therapistId}/${slotId}/slots`)
      location.reload();

    },
    async loadEvents() {
      console.log(this.user.role)
      try {
        let response;

        if (this.user.role === 'Patient' && this.$route.path === `/therapist/${this.therapistId}/appointments`) {

          response = await axios.get(`${API_BASE}/api/Therapist/${this.therapistId}/slots/available`, {
            headers: {
              'Authorization': 'Bearer ' + this.token,
              'Role': 'Patient'
            }
          });

        } else if (this.user.role === 'Patient' && this.$route.path === `/appointments`) {
          response = await axios.get(`${API_BASE}/api/Patients/${this.user.tableUserId}/appointments`, {
            headers: {
              'Authorization': 'Bearer ' + this.token,
              'Role': 'Patient'
            }
          })

        } else if (this.user.role === 'Therapist') {
          const [appointmentsResponse, slotsResponse] = await Promise.all([
            axios.get(`${API_BASE}/api/therapist/${this.user.tableUserId}/appointments`, {
              headers: {
                'Authorization': 'Bearer ' + this.token,
                'Role': 'Therapist'
              }
            }),
            axios.get(`${API_BASE}/api/therapist/${this.user.tableUserId}/slots`, {
              headers: {
                'Authorization': 'Bearer ' + this.token,
                'Role': 'Therapist'
              }
            })

          ]);


          response = [...appointmentsResponse.data, ...slotsResponse.data];
        } else {
          // Handle unrecognized role
          console.error('Unrecognized user role:', this.user.role);
          return;
        }
        let events;
        if (this.user.role == 'Therapist') {
          events = response.map(event => ({
            ...event,
            // start: new Date(event.startTime).toISOString().toLocaleString('en-US', { timeZone: 'Europe/Bucharest' }),
            // end: new Date(event.endTime).toISOString().toLocaleString('en-US', { timeZone: 'Europe/Bucharest' }),
            start: new Date(event.startTime),
            end: new Date(event.endTime),
            backColor: event.status === 'Accepted' ? 'green' : (event.status === 'Pending' ? 'red' : 'orange'), //slot color
          }));
        }
        else {
          events = response.data.map(event => ({
            ...event,
            // start: new Date(event.startTime).toISOString(),
            // end: new Date(event.endTime).toISOString(),
            start: new Date(event.startTime),
            end: new Date(event.endTime),
            backColor: event.status === 'Accepted' ? 'green' : 'orange', // Set the desired background color for the slots
          }));
          console.log(events)
          this.calendar.events.list = events; // Replace the events with the new list
          this.calendar.update();

        }


        this.calendar.events.list = events; // Replace the events with the new list
        this.calendar.update();
      } catch (error) {
        console.error('Error loading events:', error);
      }
    }

  },
  mounted() {
    this.therapistId = this.$route.params.tableUserId;
    this.loadEvents();
    console.log(Intl.DateTimeFormat().resolvedOptions().timeZone)
  },
};
</script>

<style scoped lang="scss">
.wrap {
  display: flex;
  flex-direction: row;
}

.left {
  flex: 1;
  margin: 0 10px 10px 0;
}

.content {
  flex: 4;
}

/* -------------- */


.navigator_default_main {
  border-radius: 15px;
  overflow: hidden;
  /* Ensure that the entire element is visible */
  position: relative;
  /* Adjust the positioning */
  z-index: 1;

}

.navigator_default_title,
.navigator_default_titleleft,
.navigator_default_titleright {

  background: red !important;
}

.calendar_default_main {
  border-radius: 15px;
  overflow: hidden;
  /* Ensure that the entire element is visible */
  position: relative;
  /* Adjust the positioning */
  z-index: 1;
}

@media (max-width: 960px) {
  .wrap {
    flex-direction: column;
  }

  .left {
    flex: 1;
    padding-right: 10px;
  }

  .content {
    flex: 4;
  }
}</style>