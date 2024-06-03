import { Navigate, Outlet, useLocation } from "react-router-dom";
import useAuth from "../../hooks/useAuth"
import useRefreshToken from "../../hooks/useRefreshToken";
import useAuthStore from "../../Store/useAuthStore";


const RequireAuth = () => {
  const auth = useAuthStore(state => state.auth);

  return (
    auth?.accessToken ?
      <Outlet /> :
      <Navigate to='/login' replace />
  )
}

export default RequireAuth