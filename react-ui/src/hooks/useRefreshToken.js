import useSWRImmutable from 'swr/immutable'
import { instance } from '../api/axios';
import useAuth from './useAuth';
import { useEffect } from 'react';

const refreshUrl = 'auth/refresh'

const useRefreshToken = () => {
    const { setAuth } = useAuth();
    
    const refresh = async (signal = null) => {
        var token = null;
        await instance.get(refreshUrl,
        {
            signal: signal
        })
        .then(res => {
            token = res?.data?.accessToken
            setAuth(res?.data)
        })
        .catch(error => {
            setAuth({})
        });
        return token
    }
    return refresh;
};

export default useRefreshToken;