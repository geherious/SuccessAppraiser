import { create } from "zustand";
const refreshStore = create((set) => ({
  isRefreshing: false,
  setIsRefreshing: (isRefreshing) => set(() => ({ isRefreshing: isRefreshing })),
  subscribers: [],
  addSubscriber: (callback) => set((state) => ({ subscribers: [...state.subscribers, callback] })),
  clearSubscribers: () => set(() => ({ subscribers: [] })),
}));

export default refreshStore;