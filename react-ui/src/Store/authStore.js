import { create } from "zustand";

const authStore = create((set) => ({
  auth: {},
  setAuth: (auth) => set(() => ({ auth: auth })),
  persist: JSON.parse(localStorage.getItem("persist")) || false,
  setPersist: (persist) => set(() => ({ persist: persist })),
}));

export default authStore;