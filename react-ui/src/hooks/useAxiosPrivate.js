import { axiosPrivateInstance } from "../api/axios";
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
    const requestIntercept = axiosPrivateInstance.interceptors.request.use(
      config => {
        config.headers['Authorization'] = `Bearer ${auth?.accessToken}`;
        return config;
      }, (error) => Promise.reject(error)
    );

    const responseIntercept = axiosPrivateInstance.interceptors.response.use(
      response => response,
      async (error) => {
        let config = error?.config;
        if (error?.response?.status === 401 && !config?.sent) {
          config.sent = true;
          let newAccessToken = refresh(controller.signal);
          config.headers['Authorization'] = `Bearer ${newAccessToken}`;
          return axiosPrivateInstance(config);
        }
        return Promise.reject(error);
      }
    );

    setIsConfiguring(false);

    return () => {
      axiosPrivateInstance.interceptors.request.eject(requestIntercept);
      axiosPrivateInstance.interceptors.response.eject(responseIntercept);
      setIsConfiguring(false);
      controller.abort();
    }
  }, [auth])

  return { axiosPrivate: axiosPrivateInstance, isConfiguring };
}

export default useAxiosPrivate;