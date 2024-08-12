import React from 'react'
import { useLocation, Navigate } from 'react-router-dom'
import authStore from '../../Store/authStore';

const Welcome = () => {

  const auth = authStore(state => state.auth);
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