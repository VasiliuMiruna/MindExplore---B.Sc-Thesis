import {createRouter, createWebHistory} from 'vue-router';
import LoginPage from '../views/LoginPage.vue';
import RegisterPage from '../views/RegisterPage.vue';
import HomePage from '../views/HomePage.vue';
import CarierePage from '../views/CarierePage.vue';
import RegisterTherapistPage from '@/views/RegisterTherapistPage.vue';
import {useUserStore} from '@/store/user';
// import AboutPage from '../views/AboutPage.vue';


// Define routes
const routes = [
    {
        path: '/',
        redirect: '/home' 
    },
    {path: '/login', name: 'Login', component: LoginPage},
    {path: '/register', name: 'Register', component: RegisterPage},
    {path: '/registerTherapist', name: 'RegisterTherapist', component: RegisterTherapistPage},
    {path: '/home', name: 'Home', component: HomePage},
    {path: '/cariere', name: 'Cariere', component: CarierePage},
    {path: '/therapists', name: "Therapists", component: () => import("../views/TherapistsPage.vue")},
    {path: '/appointments', component: () => import("../views/AppointmentsPage.vue")},
    {path: '/conversations/:id', component: () => import("../views/ConversationsPage.vue")},
    {path: '/notes', component: () => import("../views/NotesPage.vue")},
    {path: '/personalTherapists', component:() => import("../views/MyTherapistsPage.vue")},
    {path: '/test', component: () => import("../views/TestPage.vue")}, //sa pun si id ul pacientului
    {path: '/:id/test', component: () => import("../views/TestResultsPage.vue")},
    {path: '/:recommendedTherapists', component: () => import("../views/RecommendedTherapistsPage.vue")},
    {path: '/admin/therapists', component: () => import("../views/admin/TherapistsPage.vue")},
    {path: '/admin/patients', component: () => import("../views/admin/PatientsPage.vue")},
    {path: '/therapist/patients', component: () => import("../views/therapist/PatientsPage.vue")},
    {
        path: '/therapist/patients/:id/conversation',
        component: () => import("../views/therapist/ConversationsPage.vue")
    },
    {path: '/therapist/:tableUserId/appointments', component: () => import("../views/therapist/AppointmentsPage.vue"), props:true},
    {path: '/profile', component: () => import("../views/ProfilePage.vue")},
];

// Create router instance
const router = createRouter({
    history: createWebHistory(),
    routes
    
});

router.beforeEach((to, from, next) => {
    const userStore = useUserStore();
    console.log(to)
    if (!["Login", "Register", "RegisterTherapist" ,"Therapists", "Home", "Cariere"].includes(to.name) && !userStore.user.userId) {
        console.log("navigating")
        return next("/login")
    } else if (to.path.match(/\/\d+\/test$/) && userStore.user.role !== "Therapist") {
        // Check if the user is trying to access the "Test" page and they are not a therapist
        return next("/home"); 
    } else if(to.path.match("/personalTherapists") && userStore.user.role === "Therapist") 
    {
        return next("/home"); 
    } else if(to.path.match("/therapist/patients") && userStore.user.role !== "Therapist") 
    {
        return next("/home"); 
    } 
    else {
        next();
    }
})

export default router;
