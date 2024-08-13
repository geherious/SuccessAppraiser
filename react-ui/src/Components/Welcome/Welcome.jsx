import React from 'react'
import { useLocation, Navigate, useNavigate } from 'react-router-dom'
import authStore from '../../Store/authStore';
import styles from './Welcome.module.css';
import rulerIcon from '../../assets/images/home/ruler-icon.svg';
import templateIcon from '../../assets/images/home/template-icon.svg';
import markIcon from '../../assets/images/home/mark-icon.svg';
import goalIcon from '../../assets/images/home/goal-icon.svg';

const Welcome = () => {

  const auth = authStore(state => state.auth);
  const location = useLocation();
  const navigate = useNavigate();

  const login = () => {
    navigate('/login');
  }
  
  const register = () => {
    navigate('/register');
  }

  return (
    <>
      {auth.username ?
        (<Navigate to="/home" state={{ from: location }} replace />)
        :
        (
          <div className={styles.welcome}>
            <div className='container d-flex flex-column'>
              <div className=' flex-grow-1'>
                <div className={styles.header}>
                  <span className={styles.logo_span}>S'APP</span>
                  <div className={styles.account}>
                    <button onClick={login}>Log in</button>
                    <button onClick={register}>Sign up</button>
                  </div>
                </div>
                <div className={styles.welcome_text}>
                  Measure your&nbsp;
                  <span className={styles.highlight}>success</span>
                  &nbsp;
                  <img src={rulerIcon} className={styles.ruler_image} />
                </div>
                <div className={styles.features}>
                  <div>
                    <img src={templateIcon} />
                    <span>Create unique colorful templates to suit your needs</span>
                  </div>
                  <div>
                    <img src={markIcon} />
                    <span>Mark your days and keep track of your progress</span>
                  </div>
                  <div className={styles.last_item}>
                    <img src={goalIcon} />
                    <span>Achieve goals and stay motivated for further growth</span>
                  </div>
                </div>
              </div>
              <div className={styles.footer}>
                © Success Appraiser 2024
              </div>
            </div>
          </div>
        )}
    </>
  )
}

export default Welcome