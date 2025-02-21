<template>
    <div class="page">
        <PageTitleBar title="Programări"/>
        <div class="page-wrapper">
            
            <WeekCalendar/>
            <v-dialog
            v-model="dialog"
            v-if="this.user.role === 'Therapist'"
            persistent
            width="auto"
            >
                <template v-slot:activator="{ props }">
                    <v-btn
                    
                    color="primary"
                    icon="fas fa-info"
                    v-bind="props"
                    >
                    
                    </v-btn>
                    
                </template>
                <v-card>
                    <v-card-title class="text-h5">
                    Cum funcționează calendarul?
                    </v-card-title>
                    <v-card-text>
                        Tot ce trebuie sa faci este să selectezi intervalele orare în care esti disponibil pentru sesiunile de terapie. 
                        Pacienții care vor intra pe pagina ta le vor vedea și își vor putea alege unul convenabil. Intervalele libere sunt colorate 
                        <span style="color: orange; font-weight: bold;">portocaliu</span>.
                    </v-card-text>
                    <v-card-text>
                        Odată ce un pacient alege să se programeze într-un interval orar, acesta se va colora în 
                        <span style="color: red; font-weight: bold;">roșu</span> și ai opțiunea de a accepta sau nu acea programare.
                        O programare acceptată va fi colorată cu 
                        <span style="color: green; font-weight: bold;">verde</span>.
                        
                    </v-card-text>
                    <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn
                        color="green-darken-1"
                        variant="text"
                        @click="dialog = false"
                    >
                        Inchide
                    </v-btn>
                    </v-card-actions>
            </v-card>
            </v-dialog>
            <v-dialog
            v-model="dialog"
            v-else-if="this.user.role === 'Patient'"
            persistent
            width="auto"
            >
            <template v-slot:activator="{ props }">
                    <v-btn
                    
                    color="primary"
                    icon="fas fa-info"
                    v-bind="props"
                    >
                    
                    </v-btn>
                    
                </template>
                <v-card>
                    <v-card-title class="text-h5">
                    Cum funcționează calendarul?
                    </v-card-title>
                    <v-card-text>
                        Tot ce trebuie să faci este să alegi unul dintre intervalele disponibile ale terapeutului, marcate cu 
                        <span style="color: orange; font-weight: bold;">portocaliu</span>.
                        În momentul în care programarea este acceptată, o vei vedea marcată cu 
                        <span style="color: green; font-weight: bold;">verde</span>, în programările tale.
                    </v-card-text>
                    <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn
                        color="green-darken-1"
                        variant="text"
                        @click="dialog = false"
                    >
                        Închide
                    </v-btn>
                    </v-card-actions>
            </v-card>
            </v-dialog>
        </div>
            
        </div>
    </template>
    
<script>
import PageTitleBar from '@/components/PageTitleBar.vue';
import WeekCalendar from '@/components/WeekCalendar.vue';
import {mapState} from "pinia";
import {useUserStore} from "@/store/user";
export default {
    data () {
      return {
        dialog: false,
        
      }
    },
    components: {
            PageTitleBar,
            WeekCalendar
    },
    computed: {
    ...mapState(useUserStore, ['user']),
    
  },
}

   
</script>