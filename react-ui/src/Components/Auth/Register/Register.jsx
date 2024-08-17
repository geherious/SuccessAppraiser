import './Register.css'
import { useState, useEffect, useRef } from "react";
import { instance } from '../../../api/axios';
import authStore from '../../../Store/authStore';
import { Navigate } from 'react-router-dom';
import { registerEndpoint } from '../../../api/authApi';

const USERNAME_REGEX = /^[a-zA-z][a-zA-z0-9-_]{2,}$/;
const PASSWORD_REGEX = /^(?=.*?\d)(?=.*?[a-zA-Z])[a-zA-Z\d-]{6,}$/;
const EMAIL_REGEX = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;

const Register = () => {
  const userRef = useRef();
  const errRef = useRef();

  const [user, setUser] = useState("");
  const [validName, setValidName] = useState(false);
  const [userFocus, setUserFocus] = useState(false);

  const [email, setEmail] = useState("");
  const [validEmail, setValidEmail] = useState(false);
  const [emailFocus, setEmailFocus] = useState(false);

  const [pwd, setPwd] = useState('');
  const [validPwd, setValidPwd] = useState(false);
  const [pwdFocus, setPwdFocus] = useState(false);

  const [matchPwd, setMatchPwd] = useState('');
  const [validMatch, setValidMatch] = useState(false);
  const [matchFocus, setMatchFocus] = useState(false);

  const [errMsg, setErrMsg] = useState('');
  const [success, setSuccess] = useState(false);

  const auth = authStore(state => state.auth);


  useEffect(() => {
    const result = USERNAME_REGEX.test(user);
    setValidName(result);
  }, [user])

  useEffect(() => {
    setValidPwd(PASSWORD_REGEX.test(pwd));
    setValidMatch(pwd === matchPwd);
  }, [pwd, matchPwd])

  useEffect(() => {
    setValidEmail(EMAIL_REGEX.test(email));
  }, [email])

  useEffect(() => {
    setErrMsg('');
  }, [user, pwd, matchPwd, email])

  const handleSubmit = async (e) => {
    e.preventDefault();
    const v1 = USERNAME_REGEX.test(user);
    const v2 = PASSWORD_REGEX.test(pwd);
    const v3 = EMAIL_REGEX.test(email);

    if (!v1 || !v2 || !v3) {
      setErrMsg("Provided data is invalid");
      return;
    }

    try {
      const response = await instance.post(registerEndpoint,
        JSON.stringify({ username: user, email, password: pwd }),
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );
      setSuccess(true);

    } catch (err) {
      if (!err?.response) {
        setErrMsg("No server response");
      } else if (err.response?.status === 409) {
        setErrMsg('This username or email is already taken')
      } else {
        setErrMsg("Something went wrong");
      }

      errRef.current.focus();
    }

  }

  return (
    <>
    {auth.username ? <Navigate to="/home" /> : 
      success ? (
        <section>
          <h1>Success</h1>
          <p>
            <a href='/login'>Sign In</a>
          </p>
        </section>
      ) : (
        <section className="regsection d-flex flex-column">
          <h1>Sign Up</h1>
          <p ref={errRef} className={"text-danger" + errMsg ? "errmsg" :
            "hide"}>{errMsg}</p>

          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label className="form-label" htmlFor="email">
                Email:&nbsp;
              </label>
              <input
                type="text"
                id="email"
                className="form-control"
                autoComplete="username"
                onChange={(e) => setEmail(e.target.value)}
                required
                onFocus={() => setEmailFocus(true)}
                onBlur={() => setEmailFocus(false)}
              />
              <p className={emailFocus && email && !validEmail ? "form-text text-muted" : "hide"}>
                Should be valid email. <br />
              </p>
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="username">
                Username:&nbsp;
              </label>
              <input
                type="text"
                id="username"
                ref={userRef}
                className="form-control"
                autoComplete="off"
                onChange={(e) => setUser(e.target.value)}
                required
                onFocus={() => setUserFocus(true)}
                onBlur={() => setUserFocus(false)}
              />
              <p className={userFocus && user && !validName ? "form-text text-muted" : "hide"}>
                3 to 25 characters. <br />
                Must begin with a letter <br />
              </p>
            </div>
            <div className="form-group">
              <label htmlFor="password">
                Password:&nbsp;
              </label>
              <input
                type="password"
                id="password"
                className="form-control"
                onChange={(e) => setPwd(e.target.value)}
                value={pwd}
                required
                onFocus={() => setPwdFocus(true)}
                onBlur={() => setPwdFocus(false)}
              />
              <p className={pwdFocus && !validPwd ? "form-text text-muted" : "hide"}>
                Must be at least 6 characters.<br />
                Must include letters and numbers.<br />
              </p>
            </div>
            <div className="form-group">
              <label htmlFor="confirm_pwd">
                Confirm Password:&nbsp;
              </label>
              <input
                type="password"
                id="confirm_pwd"
                onChange={(e) => setMatchPwd(e.target.value)}
                className="form-control"
                value={matchPwd}
                required
                onFocus={() => setMatchFocus(true)}
                onBlur={() => setMatchFocus(false)}
              />
              <p className={matchFocus && !validMatch ? "instructions" : "hide"}>

                Must match the first password input field.
              </p>
            </div>
            <button className='btn btn-primary mt-1' disabled={!validName || !validPwd || !validMatch ? true : false}>Sign Up</button>
          </form>

          <div className='alreadyLoggedIn'>
            Already registered? <br />
            <a href="/login">Sign in</a>
          </div>
        </section>
      )}
    </>
  )
}

export default Register