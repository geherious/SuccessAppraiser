import { Navigate, Outlet } from "react-router-dom";
import authStore from "../../Store/authStore";


const RequireAuth = () => {
  const auth = authStore(state => state.auth);

  return (
    auth.logout ?
      <Navigate to='/' replace /> :
      auth?.accessToken ?
        <Outlet /> :
        <Navigate to='/login' replace />
  )
}

export default RequireAuth