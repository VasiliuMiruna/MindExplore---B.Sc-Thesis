import '@mdi/font/css/materialdesignicons.css'
import { createApp } from 'vue'
import App from './App.vue'
import { createPinia } from 'pinia'
import router from './router';
import '@fortawesome/fontawesome-free/js/all'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import 'vuetify/dist/vuetify.min.css'
import { useMiscStore } from './store/misc';




const pinia = createPinia()

// Create Vue app instance
const app = createApp(App);
// Use Vue router
app.use(router);
app.use(pinia);
const $misc = useMiscStore()
$misc.loadTheme()
const vuetify = createVuetify({
    theme: {
        defaultTheme: $misc.theme
    },
    components,
    directives
})
app.use(vuetify);




// Mount the Vue app
app.mount('#_app');