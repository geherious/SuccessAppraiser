import { create } from "zustand";

const useHomeStore = create((set) => ({
  activeGoal: null,
  currentDateArea: new Date(new Date().getFullYear(), new Date().getMonth(), 1),
  modalIsActive: false,
  modalDate: new Date(),
  setActiveGoal: (goal) => set({ activeGoal: goal }),
  setCurrentDateArea: (dateArea) => set({ currentDateArea: dateArea }),
  setModalDate: (date) => set({ modalDate: date }),
  setModalIsActive: (isActive) => set({ modalIsActive: isActive }),
}));

export default useHomeStore;