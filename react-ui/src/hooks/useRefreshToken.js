import authStore from '../Store/authStore';
import { instance } from '../api/axios';

const refreshUrl = 'auth/refresh'

const useRefreshToken = () => {
  const refresh = async () => {
    const token = await instance.get(refreshUrl).then((response) => {
      authStore.setState({ auth: response.data });
      return response.data.accessToken
    }).catch((error) => {
      authStore.setState({ auth: {} });
      return ""
    })
    return token
  }
  return refresh;

}

export default useRefreshToken;