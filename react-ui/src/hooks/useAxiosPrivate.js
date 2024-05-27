import { axiosPrivate } from "../api/axios";
import { useEffect, useState } from "react";
import useRefreshToken from "./useRefreshToken";
import useAuth from "./useAuth";

const useAxiosPrivate = () => {
  const refresh = useRefreshToken();
  const { auth, setAuth } = useAuth();
  const [isConfiguring, setIsConfiguring] = useState(true);
  const controller = new AbortController();

  useEffect(() => {
    setIsConfiguring(true);
    const requestIntercept = axiosPrivate.interceptors.request.use(
      config => {
        config.headers['Authorization'] = `Bearer ${auth?.accessToken}`;
        return config;
      }, (error) => Promise.reject(error)
    );

    const responseIntercept = axiosPrivate.interceptors.response.use(
      response => response,
      async (error) => {
        let config = error?.config;
        if (error?.response?.status === 401 && !config?.sent) {
          config.sent = true;
          let newAccessToken = refresh(controller.signal);
          config.headers['Authorization'] = `Bearer ${newAccessToken}`;
          return axiosPrivate(config);
        }
        return Promise.reject(error);
      }
    );

    setIsConfiguring(false);

    return () => {
      axiosPrivate.interceptors.request.eject(requestIntercept);
      axiosPrivate.interceptors.response.eject(responseIntercept);
      controller.abort();
    }
  }, [auth])

  return { axiosPrivate, isConfiguring };
}

export default useAxiosPrivate;