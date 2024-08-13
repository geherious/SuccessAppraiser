import authStore from "../Store/authStore";
import useAxiosPrivate from "./useAxiosPrivate";


const useAuth = () => {

  const axiosPrivate = useAxiosPrivate();
  const setAuth = authStore(state => state.setAuth);
  const setPersist = authStore(state => state.setPersist);

  const logout = async () => {
    await axiosPrivate.get('auth/logout');
    setPersist(false);
    setAuth({logout: true});
  }

  return logout;
}

export default useAuth