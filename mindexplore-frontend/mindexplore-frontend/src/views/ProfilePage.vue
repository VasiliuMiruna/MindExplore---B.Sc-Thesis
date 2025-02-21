<template>
  <div class="page">
    <PageTitleBar title="Profil" />
    <div class="page-wrapper">
      <v-form v-model="valid">
        <v-container>
          <v-row>
            <v-col cols="12" md="12">
              <v-text-field
                v-model="firstname"
                :rules="nameRules"
                :counter="10"
                label="Prenume"
              ></v-text-field>
            </v-col>

            <v-col cols="12" md="12">
              <v-text-field
                v-model="lastname"
                label="Nume"
              ></v-text-field>
            </v-col>

            <v-col cols="12" md="12">
              <v-select
                v-model="age"
                :items="ageOptions"
                label="Varsta"
              ></v-select>
            </v-col>
          </v-row>

          
        </v-container>

        <v-container v-if="this.user.role === 'Therapist'">
          <v-row>
            <v-col cols="12" md="12">
              <v-text-field
                v-model="quote"
                label="Citat"
              ></v-text-field>
            </v-col>

            <v-col cols="12" md="12">
              <v-text-field
                v-model="city"
                label="Oraș"
              ></v-text-field>
            </v-col>

            <v-col cols="12" md="12">
              <v-text-field
                v-model="price"
                label="Pret (lei)"
              ></v-text-field>
            </v-col>

            <v-col cols="12" md="12">
              <v-select
                v-model="rroma"
                :items="['Da', 'Nu']"
                label="Esti de etnie rroma?"
              ></v-select>
            </v-col>

            <v-col cols="12" md="12">
              <v-select
                v-model="hungarian"
                :items="['Da', 'Nu']"
                label="Esti de etnie maghiara?"
              ></v-select>
            </v-col>

            <v-col cols="12" md="12">
              <v-select
                v-model="lgbt"
                :items="['Da', 'Nu']"
                label="Esti membru LGBTQ+?"
              ></v-select>
            </v-col>

            <v-col cols="12" md="12">
              <v-select
                v-model="religious"
                :items="['Da', 'Nu']"
                label="Esti religios?"
              ></v-select>
            </v-col>
       
          </v-row>
          <v-row>
            <h3>In ce esti specializat?</h3>
            <v-checkbox
              v-for="specialty in specialtyOptions"
              :key="specialty.value"
              :label="specialty.text"
              :value="specialty.value"
              :checked="selectedSpecialties.includes(specialty.value)"
              @change="updateSelectedSpecialties($event.target.value)"
            ></v-checkbox>
          </v-row>
        </v-container>
        <div v-if="this.user.role === 'Therapist'">
          <v-btn @click="updateProfileTherapist">Actualizează-ți profilul</v-btn>
        </div>
        <div v-else>
          <v-btn @click="updateProfilePatient">Actualizează-ți profilul</v-btn>
        </div>
      </v-form>
    </div>
    <v-dialog v-model="dialogVisible" max-width="500px">
      <v-card>
        <v-card-title>Profil actualizat cu succes!</v-card-title>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { useUserStore } from '@/store/user';
import { mapState } from 'pinia';
import { VTextField } from 'vuetify';
import PageTitleBar from '../components/PageTitleBar.vue';
import axios from 'axios';
import { API_BASE } from '@/services/constants';

export default {
  components: {
    PageTitleBar,
    VTextField,
  },
  computed: {
    ...mapState(useUserStore, ['user']),
    token() {
      return localStorage.getItem("token");
    },
    specialtyOptions() {
      return [
        { value: 3, text: 'depresie' },
        { value: 4, text: 'anxietate' },
        { value: 5, text: 'relații' },
        { value: 6, text: 'conflicte familiale' },
        { value: 7, text: 'traume și abuz' },
        { value: 8, text: 'LGBTQ+' },
        { value: 9, text: 'doliu și pierdere' },
        { value: 10, text: 'tulburări de alimentație' },
        { value: 11, text: 'dependențe' },
        { value: 12, text: 'stima de sine și încredere' },
        { value: 13, text: 'consiliere în carieră' },
        { value: 14, text: 'gestionarea stresului' },
        { value: 15, text: 'gestionarea furiei' },
        { value: 16, text: 'parenting' },
        { value: 17, text: 'ADHD' },
        { value: 18, text: 'OCD' },
        { value: 19, text: 'PTSD' },
        { value: 20, text: 'tulburări de somn' },
        { value: 21, text: 'Creștinism' },
        { value: 22, text: 'Iudaism' },
        { value: 23, text: 'Islam' },
      ];
    },
  },
  data: () => ({
    dialogVisible: false,
    firstname: '',
    lastname: '',
    email: '',
    password: '',
    age:null,
    ageOptions: Array.from({ length: 87 }, (_, index) => index + 12),
    quote: '',
    nationality: '',
    city: '',
    price: null,
    lgbt:null,
    religious:null,
    rroma: null,
    hungarian:null,
    existingSpecialties: [],
    selectedSpecialties: [],
    specialties: [],
    emailRules: [
      value => {
        if (value) return true;

        return 'E-mail is requred.';
      },
      value => {
        if (/.+@.+\..+/.test(value)) return true;

        return 'E-mail must be valid.';
      },
    ],
  }),
  mounted() {
    this.fetchExistingSpecialties();
    this.fetchTherapistDetails();
  },
  methods: {
    async fetchExistingSpecialties() {
      try {
        const response = await axios.get(
          `${API_BASE}/api/Therapist/therapistSpecialties/${this.user.tableUserId}`
        );
        const existingSpecialtiesIds = response.data;
        this.existingSpecialties = this.specialtyOptions
          .filter((specialty) => existingSpecialtiesIds.includes(specialty.value))
          .map((specialty) => specialty.value); // Store as array of ints
        this.selectedSpecialties = this.existingSpecialties.slice();
      } catch (error) {
        console.error('Error fetching existing specialties:', error);
      }
    },
    async fetchTherapistDetails() {
      if(this.user.role === 'Therapist')
      {
        try {
        const response = await axios.get(
          `${API_BASE}/api/Therapist/${this.user.tableUserId}`
        );
        const therapist = response.data;

        this.firstname = therapist.firstName || '';
        this.lastname = therapist.lastName || '';
        this.age = therapist.age || null;
        this.quote = therapist.quote || '';
        this.city = therapist.city || '';
        this.price = therapist.price || null;
        if(therapist.isMemberOfLGBT === true)
          this.lgbt = "Da";
        if(therapist.isMemberOfLGBT === false)
          this.lgbt = "Nu";
        if(therapist.isNotReligious == true)
          this.religious = "Nu";
        if(therapist.isNotReligious == false)
          this.religious = "Da";
        if(therapist.isRRoma === true)
          this.rroma = "Da";
        if(therapist.isRRoma === false)
          this.rroma= "Nu";
        if(therapist.isHungarian === true)
          this.hungarian = "Da";
        if(therapist.isHungarian === false)
          this.hungarian = "Nu";
        // Update other fields with therapist details
      } catch (error) {
        console.error('Error fetching therapist details:', error);
      }

      }
      else if(this.user.role==='Patient')
      {
        try {
        const response = await axios.get(
          `${API_BASE}/api/Patients/${this.user.tableUserId}`
        );
        const patient = response.data;
        this.firstname = patient.firstName || '';
        this.lastname = patient.lastName || '';
        this.age = patient.age || null;

      } catch (error) {
        console.error('Error fetching therapist details:', error);
      }
    }
  },
    updateSelectedSpecialties(value) {
  if (this.selectedSpecialties.includes(value)) {
    this.selectedSpecialties = this.selectedSpecialties.filter(specialty => specialty !== value);
  } else {
    this.selectedSpecialties.push(value);
  }
},

    async updateProfileTherapist() {
    try {
      const response = await axios.get(`${API_BASE}/api/Therapist/${this.user.tableUserId}`);
      const therapistData = response.data;
      
      therapistData.firstName = this.firstname;
      therapistData.lastName = this.lastname;
      therapistData.age = this.age;
      therapistData.quote = this.quote;
      therapistData.city = this.city;
      therapistData.price = this.price;
      
      if(this.rroma !== null){
        if(this.rroma === 'Da')
          therapistData.isRRoma = true;
        else therapistData.isRRoma = false;
      }
      
      if(this.lgbt !== null){
        if(this.lgbt === 'Da')
          therapistData.isMemberOfLGBT = true;
        else therapistData.isMemberOfLGBT = false;
      }
      if(this.religious !== null){
        if(this.religious === 'Da')
          therapistData.isNotReligious = false;
        else therapistData.isNotReligious = true;
      }
      if(this.hungarian !== null){
        if(this.hungarian === 'Da')
          therapistData.isHungarian = true;
        else therapistData.isHungarian = false;
      }
      console.log(this.selectedSpecialties)
      //therapistData.specialties = this.selectedSpecialties.map((specialty) => specialty.value);
      therapistData.specialties = this.selectedSpecialties;

      console.log(therapistData.specialties)
      this.user.name = therapistData.firstName + ' ' + therapistData.lastName,
      localStorage.setItem('name', this.user.name);
     
      

      await axios.put(`${API_BASE}/api/Therapist/${this.user.userId}`, therapistData,
        {
          headers: {
              'Authorization': 'Bearer ' + this.token,
              Role: 'Therapist',
            }}
            );
          
      this.dialogVisible = true;
      
      console.log("succes")
    } catch (error) {
      console.error('Error updating therapist profile:', error);
     
    }
  },
  async updateProfilePatient() {
    try {
      const response = await axios.get(`${API_BASE}/api/Patients/${this.user.tableUserId}`);
      const PatientData = response.data;
      
      if(this.firstname != null) PatientData.firstName = this.firstname;
      if(this.lastname != null) PatientData.lastName = this.lastname;
      PatientData.age = this.age;
      console.log(this.age)
      console.log(PatientData.age)
      //if(this.city != null)PatientData.city = this.city;
     
      console.log(PatientData.firstName + ' ' + PatientData.lastName,)
      this.user.name = PatientData.firstName + ' ' + PatientData.lastName,
      localStorage.setItem('name', this.user.name);
      await axios.put(`${API_BASE}/api/Patients/${this.user.userId}`, PatientData,
        {
          headers: {
              'Authorization': 'Bearer ' + this.token,
              Role: 'Patient',
            }}
            );
          
     this.dialogVisible = true;
      
    } catch (error) {
      console.error('Error updating patient profile:', error);
     
    }
  },
  },
};
</script>

<style scoped>
</style>
