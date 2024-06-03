import { create } from "zustand";

const useAuthStore = create((set) => ({
  auth: {},
  setAuth: (auth) => set({ auth }),
  persist: JSON.parse(localStorage.getItem("persist")) || false,
  setPersist: (persist) => set({ persist }),
}));

export default useAuthStore;