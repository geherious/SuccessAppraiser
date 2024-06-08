import authStore from '../Store/authStore';
import refreshStore from '../Store/refreshStore';
import { axiosPrivateInstance } from '../api/axios';
import useRefreshToken from './useRefreshToken';


const useAxiosPrivate = () => {
  const refresh = useRefreshToken();

  const onAccessTokenFetched = async (access_token) => {
    refreshStore.getState().subscribers.forEach(callback => callback(access_token));
    refreshStore.setState({ subscribers: [] });
  };


  axiosPrivateInstance.interceptors.request.use(
    config => {
      const token = authStore.getState().auth.accessToken;
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    error => {
      return Promise.reject(error);
    }
  );

  axiosPrivateInstance.interceptors.response.use(
    response => response,
    async error => {
      const { config, response } = error;
      const originalRequest = config;
      if (response && response.status === 401 && !originalRequest._retry) {
        if (refreshStore.getState().isRefreshing) {

          // If refreshing is already happening, queue the request
          return new Promise(async (resolve) => {
            refreshStore.getState().addSubscriber(access_token => {
              originalRequest.headers.Authorization = `Bearer ${access_token}`;
              resolve(axiosPrivateInstance(originalRequest));
            });
          });
        }

        originalRequest._retry = true;
        refreshStore.setState({ isRefreshing: true });

        return new Promise(async (resolve, reject) => {
          try {
            const newToken = await refresh();
            originalRequest.headers.Authorization = `Bearer ${newToken}`;
            await onAccessTokenFetched(newToken);
            resolve(axiosPrivateInstance(originalRequest));
          } catch (err) {
            reject(err);
          } finally {
            refreshStore.setState({ isRefreshing: false });
          }
        });
      }

      return Promise.reject(error);
    }
  );
  return axiosPrivateInstance;
}

export default useAxiosPrivate;