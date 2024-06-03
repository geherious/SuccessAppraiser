import React from 'react'
import { Link, useLocation, Navigate } from 'react-router-dom'
import useAuth from '../../hooks/useAuth'
import useAuthStore from '../../Store/useAuthStore';

const Welcome = () => {

  const auth = useAuthStore(state => state.auth);
  const location = useLocation();

  return (
    <>
      {auth.username ?
        (<Navigate to="/home" state={{ from: location }} replace />)
        :
        (
          <>
            <h1>Welcome page</h1>
            <p>It's a page shown to everyone who is not registered</p>
          </>
        )}
    </>
  )
}

export default Welcome