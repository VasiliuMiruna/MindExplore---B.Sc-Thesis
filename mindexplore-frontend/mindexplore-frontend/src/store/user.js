import {defineStore} from "pinia";

export const useUserStore = defineStore('user', {
    state: () => ({
        token: "",
        user: {
            tableUserId: localStorage.getItem("tableUserId"),
            userId: localStorage.getItem("userId"),
            name: localStorage.getItem("name"),
            role: localStorage.getItem("role") // "Patient" / "Therapist" / "Admin"
        }
    }),
    actions: {
        login(token, role, name, userId, tableUserId) {
            // tableUserId - scurt
            // userId - lung
            localStorage.setItem('token', token);
            localStorage.setItem('role', role);
            localStorage.setItem('name', name);
            localStorage.setItem('tableUserId', tableUserId);
            localStorage.setItem('userId', userId);
            this.user.userId = userId;
            this.user.tableUserId = tableUserId;
            this.user.name = name;
            this.user.role = role;

        }
    }
})


