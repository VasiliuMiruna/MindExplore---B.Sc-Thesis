import { defineStore } from 'pinia';

export const useTherapistStore = defineStore({
  id: 'therapist',
  state: () => ({
    recommendedTherapists: [],
  }),
});
