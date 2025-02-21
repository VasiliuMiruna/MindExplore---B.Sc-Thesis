<template>
    <div class="title-bar">
        <h1>{{ title }}</h1>
        <div style="flex-grow: 1;"></div>
        <!-- <v-avatar class="ma-3" size="50" rounded="50">
            <v-img :src="imagePath" to="/profile"></v-img>
        </v-avatar> -->
        <router-link to="/profile">
            {{ user.name }}
        </router-link>
        
    </div>
</template>
<script>
import { useUserStore } from '@/store/user';
import { mapActions, mapState } from 'pinia';

export default {
    props: {
        title: String,
    },
    computed: {
        ...mapState(useUserStore, ['user']),
    imagePath() {
      return require('@/assets/user.png');
    },
    },
    methods: {
        ...mapActions(useUserStore, ['login'])
    }
}
</script>
<style  lang="scss">
.title-bar {
    display:flex;
    align-items: center;
    font-weight: bold;
    margin-bottom: 25px;
}
</style>