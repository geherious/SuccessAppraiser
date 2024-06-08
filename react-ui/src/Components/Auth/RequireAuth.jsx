import { Navigate, Outlet, useLocation } from "react-router-dom";
import useAuth from "../../hooks/useAuth"
import useRefreshToken from "../../hooks/useRefreshToken";
import authStore from "../../Store/authStore";


const RequireAuth = () => {
  const auth = authStore(state => state.auth);

  return (
    auth?.accessToken ?
      <Outlet /> :
      <Navigate to='/login' replace />
  )
}

export default RequireAuth