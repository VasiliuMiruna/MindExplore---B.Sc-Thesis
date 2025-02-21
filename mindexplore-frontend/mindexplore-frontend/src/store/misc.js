import { defineStore } from "pinia";

export const useMiscStore = defineStore('misc', {
    state: () => ({
        theme: "light"
    }),
    actions: {
        setTheme(variant) {
            this.theme = variant;
            localStorage.setItem("theme", variant)
            window.location.reload()
        },
        loadTheme() {
            this.theme = localStorage.getItem("theme") || "dark"
        },
        switchTheme() {
            this.theme == "dark" ? this.setTheme("light") : this.setTheme("dark")
        }
    }
})