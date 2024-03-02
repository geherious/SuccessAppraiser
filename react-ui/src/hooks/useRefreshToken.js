import { instance } from '../api/axios';
import useAuth from './useAuth';

const useRefreshToken = () => {
    const { setAuth } = useAuth();

    const refresh = async () => {
        let token = '';
        await instance.get('/auth/refresh', {
            withCredentials: true
        })
        .then(success => {
            setAuth(prev => {
                return { ...prev, accessToken: success.data.accessToken }
            });
            token = success.data.accessToken;
        })
        .catch(() => {
            token = null;
        });

        return token;
    }
    return refresh;
};

export default useRefreshToken;