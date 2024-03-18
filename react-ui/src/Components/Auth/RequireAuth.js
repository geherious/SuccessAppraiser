import { Navigate, Outlet, useLocation } from "react-router-dom";
import useAuth from "../../hooks/useAuth"
import useRefreshToken from "../../hooks/useRefreshToken";


const RequireAuth = () => {
  const {auth} = useAuth();


  return (
      auth?.accessToken ?
      <Outlet/> :
        <Navigate to='/login' replace/>
        // <p>Denied</p>
  )
}

export default RequireAuth