import './Login.css';

import { instance } from '../../../api/axios';
import { useRef, useState, useEffect } from "react"
import useAuth from '../../../hooks/useAuth';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import useAuthStore from '../../../Store/useAuthStore';

const LOGIN_URL = '/auth/login';

const Login = () => {
  const setAuth = useAuthStore(state => state.setAuth);
  const persist = useAuthStore(state => state.persist);
  const setPersist = useAuthStore(state => state.setPersist);

  const navigate = useNavigate();
  const location = useLocation();
  const from = location.state?.from?.pathname || "/";

  const emailRef = useRef();
  const errRef = useRef();

  const [email, setEmail] = useState('');
  const [pwd, setPwd] = useState('');
  const [errMsg, setErrMsg] = useState('');
  const [pending, setPending] = useState(false);

  useEffect(() => {
    emailRef.current.focus();
  }, []);

  useEffect(() => {
    setErrMsg('')
  }, [email, pwd]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setPending(true);

    try {
      const response = await instance.post(LOGIN_URL,
        JSON.stringify({ email: email, password: pwd }),
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );
      const token = response?.data.accessToken;
      if (token === null) {
        setErrMsg('Unauthorized');
        throw new Error();
      }
      setAuth({ accessToken: response.data.accessToken, username: response.data.username });
      navigate(from, { replace: true })
    } catch (err) {
      if (!err?.response) {
        setErrMsg('No Server Response');
      } else if (err.response?.status === 400) {
        setErrMsg('Missing Username or Password');
      } else if (err.response?.status === 401) {
        setErrMsg('Unauthorized');
      } else {
        setErrMsg('Login Failed');
      }
      errRef.current.focus();
    }
    setPending(false);
  }

  const togglePersist = () => {
    setPersist(prev => !prev)
  }

  useEffect(() => {
    localStorage.setItem("persist", persist);
  }, [persist])

  return (
    <section className='loginsection d-flex flex-column'>
      <p ref={errRef} className={errMsg ? "errmsg" : "hide"}>{errMsg}</p>
      <h1>Sign In</h1>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <input
            className='form-control'
            type="email"
            id="email"
            placeholder='Email'
            ref={emailRef}
            autoComplete="on"
            onChange={(e) => setEmail(e.target.value)}
            value={email}
            required
          />
        </div>
        <div className="form-group">
          <input
            type="password"
            placeholder='Password'
            className='form-control'
            id="password"
            onChange={(e) => setPwd(e.target.value)}
            value={pwd}
            required
          />
        </div>
        <div className="form-check">
          <input
            className="form-check-input"
            type="checkbox"
            value=""
            id="persist"
            onChange={togglePersist}
            checked={persist}
          />
          <label className="form-check-label" htmlFor="persist">
            Trust this device?
          </label>
        </div>
        <button disabled={pending} className='btn btn-primary'>Sign In</button>
      </form>
      <p>
        Need an Account?<br />
        <span className="line">
          <a href="#">Sign Up</a>
        </span>
      </p>
    </section>
  )
}

export default Login