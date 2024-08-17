import axios from "axios";

const baseUrl = import.meta.env.VITE_BASE_PATH;

export const instance = axios.create({
  baseURL: baseUrl,
  withCredentials: true
});

export const axiosPrivateInstance = axios.create({
  baseURL: baseUrl,
  headers: { 'Content-Type': 'application/json' },
  withCredentials: true
})