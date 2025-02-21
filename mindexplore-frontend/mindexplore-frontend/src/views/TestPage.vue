<template>
  <div class="page">
    <PageTitleBar title="Găsește un terapeut"/>
    <p class="home-subheading">
            *Întrebările sunt făcute pentru a te cunoaște mai bine
          </p>
    <div class="page-wrapper">
      <div>
        <v-progress-linear :model-value="progressValue"></v-progress-linear>
        <v-card class="mx-auto" max-width="600" min-width="600">
          <v-card-title class="text-h6 font-weight-regular justify-space-between">
            <span>{{ currentTitle }}</span>
            <v-avatar color="primary" size="24" v-text="step"></v-avatar>
          </v-card-title>

          <v-window v-model="step">
            <v-window-item v-for="(value, index) in questions" :value="index + 1" :key="`question-${index}`">
              <v-card-text>
                <h2 class="mb-5">{{ value.question }}</h2>
                <!-- <v-select label="Select" :items="value.options" v-model="value.selected"></v-select> -->
                <v-select label="Select" :items="value.options" v-model="questions[index].selected"></v-select>
              </v-card-text>
            </v-window-item>
          </v-window>
          <v-divider></v-divider>

          <v-card-actions>
            <v-btn v-if="step > 1" variant="text" @click="goToPreviousQuestion">Inapoi</v-btn>
            <v-spacer></v-spacer>
            <v-btn v-if="step < questions.length" color="primary" variant="flat" @click="goToNextQuestion">Inainte</v-btn>
            <v-btn v-if="step === questions.length" color="primary" variant="flat" @click="submitResponses">Salveaza raspunsurile</v-btn>
          </v-card-actions>
        </v-card>
      </div>
    </div>
  </div>
</template>
<script>
import PageTitleBar from '../components/PageTitleBar.vue';
import axios from 'axios';
import {API_BASE} from '@/services/constants';
import {useUserStore} from "@/store/user";
import {useTherapistStore} from "@/store/therapist";
import {mapState} from "pinia";

export default {
  components: {
    PageTitleBar

  },
  data() {
    return {
      step: 1,
      recommendedTherapists: [],
      questions: [
        {
          question: 'Ce varsta ai?',
          options: Array.from({length: 89}, (_, i) => 12 + i),
          selected: null
        },
        {
          question: 'Care este identitatea ta de gen?',
          options: [
            'Feminina',
            'Masculina',
            'Transfeminina',
            'Transmasculina',
            'Agendera',
            'Nonbinara',
            'Cisgendera',
            'Prefer sa nu raspund',
            'Nu stiu',
            'Altceva'
          ],
          selected: null
        },
        {
          question: 'Care este orientarea ta sexuala?',
          options: [
            'Heterosexual',
            'Lesbiana',
            'Gay',
            'Bi sau Pan',
            'Demisexual sau Demiromantic',
            'Asexual',
            'Queer',
            'Prefer să nu raspund',
            'Nu stiu',
            'Altceva'
          ],
          selected: null
        },
        {
          question: 'Ai dori ca terapeutul tau să fie specializat în probleme legate de comunitatea LGBTQ+?',
          options: ['Da', 'Nu'],
          selected: null
        },
        {
          question: 'Care sunt pronumele tale?',
          options: ['Ea/Ei', 'El/Ei', 'Ei/Lor', 'Altele'],
          selected: null
        },
        {
          question: 'Care este statusul relatiei tale?',
          options: ['Singur/a', 'Intr-o relatie', 'Casatorit/ă', 'Vaduv/a', 'Altul'],
          selected: null
        },
        {
          question: 'Te consideri o persoana religioasa?',
          options: ['Da', 'Nu'],
          selected: null
        },
        {
          question: 'Cu ce religie te identifici?',
          options: ['Crestinism', 'Islam', 'Iudaism', 'Altceva'],
          selected: null
        },
        {
          question: 'Ai vrea ca terapia sa fie bazata pe religie?',
          options: ['Da', 'Nu'],
          selected: null
        },
        //partea de screening depresie
        {
          question: 'In ultimele saptamani cat de des te-ai confruntat cu: Putin interes sau placere de a face activitatile curente:',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null
        },
        {
          question: 'In ultimele saptamani cat de des te-ai confruntat cu: Sentimente de tristete, demoralizare, lipsa de speranta:',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null
        },
        {
          question: 'In ultimele saptamani cat de des te-ai confruntat cu: Tulburari de somn (insomnie sau somnolenta):',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null
        },
        {
          question: 'In ultimele saptamani cat de des te-ai confruntat cu: Oboseala sau lipsa de energie:',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null
        },
        {
          question: 'In ultimele saptamani cat de des te-ai confruntat cu: Scaderea sau cresterea apetitului:',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null
        },
        {
          question: 'In ultimele saptamani cat de des te-ai confruntat cu: Parere proasta despre sine, sentimente de dezamagire a familiei:',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null
        },
        {
          question: 'In ultimele saptamani cat de des te-ai confruntat cu: Dificultati de concentrare in activitati uzuale (de exemplu cand citesti sau te uiti la televizor):',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null
        },
        {
          question: 'In ultimele saptamani cat de des te-ai confruntat cu: Vorbit sau miscari lente, lucru sesizat de altii, sau, din potriva, agitatie si neliniste:',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null
        },
        {
          question: 'In ultimele saptamani cat de des te-ai confruntat cu: Gandul ca ar fi mai bine sa mori sau sa iti faci rau:',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null
        },
        {
          question: 'In ultimele saptamani cat de des te-ai simtit nervos sau nelinistit',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null

        },
        {
          question: 'In ultimele saptamani cat de des nu te-ai putut opri din a fi ingrijorat',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null

        },
        {
          question: 'In ultimele saptamani cat de des te-ai ingrijorat prea mult despre diferite lucruri',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null

        },
        {
          question: 'In ultimele saptamani cat de des te-ai confruntat cu: dificultatea de a te relaxa',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null

        },
        {
          question: 'In ultimele saptamani cat de des ai fost atat de agitat incat ti-a fost greu sa stai linistit ',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null

        },
        {
          question: 'In ultimele saptamani cat de des ai devenit usor iritat sau iritat',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null

        },
        {
          question: 'In ultimele saptamani cat de des te-ai simtit ca si cum s-ar putea intampla ceva groaznic',
          options: [
            "Deloc",
            "In mai multe zile",
            "In mai mult de jumatate din timp",
            "Aproape in fiecare zi"
          ],
          selected: null

        },

        {
          question: 'Ai cerinte specifice in legatura cu terapeutul?',
          options: [
            'Sa fie femeie',
            'Sa fie barbat',
            'Sa apartina comunitatii LGBTQ+',
            'Sa nu fie religios',
            'Sa fie de etnie rroma',
            'Sa fie de etnie maghiara',
            'Sa aiba sub 35 de ani',
            'Sa aiba peste 55 de ani',
            'Nu am'
          ],
          selected: null
        },
        {
          question: 'Cum ai dori sa se desfasoare sedintele de terapie?',
          options: [
            'Fizic',
            'Online',
            'Nu are importanta'
          ],
          selected: null
        },
        // {
        //   question: 'In ce oras ai vrea sa se desfasoare?',
        //   options: [
        //     'Bucuresti',
        //     'Iasi',
        //     'Cluj'
        //   ],
        //   selected: null
        // },
        {
          question: 'Ce te-a facut sa consideri terapia?',
          options: [
            'M-am simtit deprimat in ultimul timp',
            'M-am simtit anxios',
            'Starea mea psihica imi afecteaza job-ul sau performanta scolara',
            'Ma chinui sa pastrez relatii',
            'Nu pot sa gasesc un scop in viata',
            'Sunt in doliu',
            'O experienta traumatica',
            'Am nevoie de suport pentru a depasesi o provocare',
            'Vreau sa am mai multa incredere in mine',
            'Vreau sa ma imbunatatesc, dar nu stiu de unde sa incep',
            'Mi-a fost recomandat de cineva(familie, prieteni, doctor)',
            'Doar explorez',
            'Altceva'
          ]
        }

      ],
      userResponses: [],
      quizCompleted: false,
      currentQuestion: 0
    };
  },
  computed: {
    activeQuestion() {
      return this.questions[this.step];
    },
    getCurrentQuestion() {
      let question = this.questions[this.currentQuestion];
      question.index = this.currentQuestion;
      return question;
    },
    ...mapState(useUserStore, ['user']),
    // currentUser() {
    //   return localStorage.getItem("tableUserId");
    // },
    progressValue() {
      return Math.ceil((this.step / this.questions.length) * 100);
    },
  },
  methods: {
    goToNextQuestion() {
      if (this.step == this.questions.length) {
        return; // not possible anyway
      }
      if (this.step == 3 && this.questions[2].selected === "Heterosexual") {
        this.step = 5;
        return;
      }
      if (this.step == 7 && this.questions[6].selected == "Nu") {
        this.step = 10;
        return;
      }
      this.step += 1;
    },
    goToPreviousQuestion() {
      if (this.step == 1) {
        return; //not possible anyway
      }
      if (this.step == 5 && this.questions[2].selected === "Heterosexual") {
        this.step = 3;
        return;
      }
      if (this.step == 9 && this.questions[5].selected == "Nu") {
        this.step = 6;
        return;
      }
      this.step -= 1;
    },
    nextQuestion() {
      if (this.currentQuestion === 2 && this.getCurrentQuestion.selected === 'Heterosexual') {
        this.currentQuestion += 2;
      } else if ((this.currentQuestion === 5 || this.currentQuestion === 6) && this.questions[this.currentQuestion].question === 'Te consideri o persoană religioasă?' && this.getCurrentQuestion.selected !== 'Da') {
        this.currentQuestion += 3;
      } else if (this.currentQuestion < this.questions.length) {
        this.currentQuestion++;
      } else {
        this.quizCompleted = true;
      }
    },
    saveResponse() {
      this.userResponses.push({
        question: this.getCurrentQuestion.question,
        selected: this.getCurrentQuestion.selected
      });
      this.nextQuestion();
    },
    // async submitResponses() {

    //   const transformedResponses = this.questions.reduce((result, response) => {
    //     result[response.question] = String(result.selected);
    //     return result;
    //   }, {});
    //   // sending responsebody to the backend
    //   try {
    //     console.log(this.user.tableUserId);
    //     console.log(transformedResponses);
    //     const response = axios.post(`${API_BASE}/api/Test/${this.user.tableUserId}`, transformedResponses)
    //     useTherapistStore().recommendedTherapists = response.data;
    //     console.log(useTherapistStore().recommendedTherapists)
    //     this.$router.push('/recommendedTherapists');
    //     // console.log(this.receiverUserId)

    //     console.log(transformedResponses)
    //   } catch (error) {
    //     console.error(error);
    //   }
    //   console.log(transformedResponses);
    // }
    async submitResponses() {
  const transformedResponses = this.questions.reduce((result, response) => {
    result[response.question] = String(response.selected);
    return result;
  }, {});

  try {
    console.log(this.user.tableUserId);
    console.log(transformedResponses);
    const response = await axios.post(`${API_BASE}/api/Test/${this.user.tableUserId}`, transformedResponses);
    useTherapistStore().recommendedTherapists = response.data;
    console.log(useTherapistStore().recommendedTherapists);
    this.$router.push('/recommendedTherapists');
    console.log(transformedResponses);
  } catch (error) {
    console.error(error);
  }
  console.log(transformedResponses);
}

  },


};

</script>

<style scoped>
.page-wrapper {
  display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
}
.v-progress-linear__background {
    border-radius: 5px !important; 
  }
.quiz-container {
  max-width: 400px;
  margin: 0 auto;
  padding: 20px;
  text-align: center;
}

.question-container {
  margin-bottom: 20px;
}

.answer-select {
  width: 100%;
  padding: 10px;
  border-radius: 5px;
  border: 1px solid #ccc;
  font-size: 16px;
}

.response-button {
  margin-top: 10px;
  padding: 10px 20px;
  border-radius: 5px;
  border: none;
  background-color: #4caf50;
  color: #fff;
  font-size: 16px;
  cursor: pointer;
}

.submit-button {
  margin-top: 20px;
  padding: 10px 20px;
  border-radius: 5px;
  border: none;
  background-color: #2196f3;
  color: #fff;
  font-size: 16px;
  cursor: pointer;
}

.quiz-completed {
  margin-top: 20px;
  font-weight: bold;
}
.home-subheading {
    font-size: 18px;
    color: #666;
    margin-bottom: 2rem;
  }
</style>
