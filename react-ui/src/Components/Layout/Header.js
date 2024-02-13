import React from 'react'
import { Link } from 'react-router-dom'
import useAuth from '../../hooks/useAuth'

const Header = () => {

    const {auth} = useAuth();
  return (
    <header className='w-100 align-items-baseline'>
        <div className='float-end'>
            {auth.username ? ("Hi, " + auth.username)
            :
            (<Link to="/login">Login</Link>)}
        </div>
    </header>
  )
}

export default Header