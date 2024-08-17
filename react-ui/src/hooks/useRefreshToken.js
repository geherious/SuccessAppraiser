import authStore from '../Store/authStore';
import { refreshEndpoint } from '../api/authApi';
import { instance } from '../api/axios';

const useRefreshToken = () => {
  const refresh = async () => {
    const token = await instance.get(refreshEndpoint).then((response) => {
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