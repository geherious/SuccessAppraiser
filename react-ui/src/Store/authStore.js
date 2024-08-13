import { create } from "zustand";

const authStore = create((set) => ({
  auth: {},
  setAuth: (auth) => set(() => ({ auth: auth })),
  persist: JSON.parse(localStorage.getItem("persist")) || false,
  setPersist: (persist) => set(() => localStorage.setItem("persist", JSON.stringify(persist))),
}));

export default authStore;