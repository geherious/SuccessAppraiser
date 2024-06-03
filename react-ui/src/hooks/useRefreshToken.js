import useSWR from 'swr';
import { instance } from '../api/axios';
import useAuth from './useAuth';
import { useEffect, useRef } from 'react';
import useAuthStore from '../Store/useAuthStore';

const refreshUrl = 'auth/refresh'

const useRefreshToken = () => {
  const setAuth = useAuthStore(state => state.setAuth);

  const aborter = useRef();
  const abort = () => aborter.current?.abort()

  const { data, isLoading, error, mutate } = useSWR(refreshUrl, async () => {
    aborter.current = new AbortController();
    return instance.get(refreshUrl , { signal: aborter.current.signal }).then((res) => res.data)
  }, {
    revalidateOnFocus: false,
    revalidateOnReconnect: false,
    revalidateIfStale: false,
    shouldRetryOnError: false,
    onError: () => {
      setAuth({});
    },
    onSuccess: (newData) => {
      console.log(newData)
      setAuth(newData);
    }
  });

  return {
    token: data?.accessToken,

    tokenIsLoading: isLoading,
    tokenError: error,
    abortToken: abort
  }
};

export default useRefreshToken;