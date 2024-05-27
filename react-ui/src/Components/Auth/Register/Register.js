import './Register.css'

import { faInfoCircle, faCheck, faTimes } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState, useEffect, useRef } from "react";
import { instance } from '../../../api/axios';

const USERNAME_REGEX = /^[a-zA-z][a-zA-z0-9-_]{2,}$/;
const PASSWORD_REGEX = /^(?=.*?\d)(?=.*?[a-zA-Z])[a-zA-Z\d-]{6,}$/;
const EMAIL_REGEX = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
const REGISTER_URL = "/auth/register";

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

  useEffect(() => {
    userRef.current.focus();
  }, [])

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
      const response = await instance.post(REGISTER_URL,
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
      {success ? (
        <section>
          <h1>Success</h1>
          <p>
            <a href='#'>Sign In</a>
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
                <FontAwesomeIcon icon={faCheck} className={validEmail ? "valid" : "hide"} />
                <FontAwesomeIcon icon={faTimes} className={!validEmail || !email ? "invalid" : "hide"} />
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
                <FontAwesomeIcon className="pe-1" icon={faInfoCircle} />
                Should be valid email. <br />
              </p>
            </div>
            <div className="form-group">
              <label className="form-label" htmlFor="username">
                Username:&nbsp;
                <FontAwesomeIcon icon={faCheck} className={validName ? "valid" : "hide"} />
                <FontAwesomeIcon icon={faTimes} className={!validName || !user ? "invalid" : "hide"} />
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
                <FontAwesomeIcon className="pe-1" icon={faInfoCircle} />
                3 to 25 characters. <br />
                Must begin with a letter <br />
              </p>
            </div>
            <div className="form-group">
              <label htmlFor="password">
                Password:&nbsp;
                <FontAwesomeIcon icon={faCheck} className={validPwd ? "valid" : "hide"} />
                <FontAwesomeIcon icon={faTimes} className={!validPwd || !pwd ? "invalid" : "hide"} />
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
                <FontAwesomeIcon className="pe-1" icon={faInfoCircle} />
                Must be at least 6 characters.<br />
                Must include letters and numbers.<br />
              </p>
            </div>
            <div className="form-group">
              <label htmlFor="confirm_pwd">
                Confirm Password:&nbsp;
                <FontAwesomeIcon icon={faCheck} className={validMatch && matchPwd ? "valid" : "hide"} />
                <FontAwesomeIcon icon={faTimes} className={!validMatch || !matchPwd ? "invalid" : "hide"} />
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
                <FontAwesomeIcon className="pe-1" icon={faInfoCircle} />
                Must match the first password input field.
              </p>
            </div>
            <button className='btn btn-primary mt-1' disabled={!validName || !validPwd || !validMatch ? true : false}>Sign Up</button>
          </form>

          <div className='alreadyLoggedIn'>
            Already registered? <br />
            <a href="#">Sign in</a>
          </div>
        </section>
      )}
    </>
  )
}

export default Register