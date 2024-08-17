import axios from "axios";

const apiPath = import.meta.env.VITE_BASE_PATH;
// const baseUrl = `http://${apiPath}`;
const baseUrl = "/api";

export const instance = axios.create({
  baseURL: baseUrl,
  withCredentials: true
});

export const axiosPrivateInstance = axios.create({
  baseURL: baseUrl,
  headers: { 'Content-Type': 'application/json' },
  withCredentials: true
})