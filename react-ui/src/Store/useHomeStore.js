import { create } from "zustand";

const useHomeStore = create((set) => ({
  activeGoal: null,
  currentDateArea: new Date(new Date().getFullYear(), new Date().getMonth(), 1),
  dateModalIsActive: false,
  dateModalDate: new Date(),
  setActiveGoal: (goal) => set({ activeGoal: goal }),
  setCurrentDateArea: (dateArea) => set({ currentDateArea: dateArea }),
  setDateModalDate: (date) => set({ dateModalDate: date }),
  setDateModalIsActive: (isActive) => set({ dateModalIsActive: isActive }),
}));

export default useHomeStore;