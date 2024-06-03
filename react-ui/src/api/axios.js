import axios from "axios";

export const instance = axios.create({
  baseURL: 'https://localhost:7127',
  withCredentials: true
});

export const axiosPrivateInstance = axios.create({
  baseURL: 'https://localhost:7127',
  headers: { 'Content-Type': 'application/json' },
  withCredentials: true
})