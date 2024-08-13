import CalendarControl from '../Calendar/CalendarControl';
import './UpBar.css';
import sideBarImage from '../../assets/images/home/hamburger-sidebar.svg';
import exitImage from '../../assets/images/home/exit-icon.svg';
import authStore from '../../Store/authStore';
import useAxiosPrivate from '../../hooks/useAxiosPrivate';
import { useLocation, useNavigate } from 'react-router-dom';
const UpBar = ({sideBarRefCol}) => {

  const setAuth = authStore(state => state.setAuth);
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();

  const toggleSideBar = () => {
    if (sideBarRefCol.current.classList.contains('hidden')) {
      sideBarRefCol.current.classList.remove('hidden');
    }
    else {
      sideBarRefCol.current.classList.add('hidden');
    }
  }

  const exit = async () => {
    await axiosPrivate.get('auth/logout');
    setAuth({logout: true});
  }

  return (
    <div className='row no-gutters'>
      <div className='col-lg-2 d-none d-lg-flex p-0'>
        <span className='logo-span'>
          S'APP
        </span>
      </div>
      <div className='col p-0 d-flex'>
        <div className='side-button-container d-lg-none'>
          <button onClick={toggleSideBar} type='button' className='side-button'>
            <img src={sideBarImage} alt='Toggle' className='side-icon' />
          </button>
        </div>
        <CalendarControl />
        <div className='log-out-button-container'>
          <button onClick={exit} type='button' className='log-out-button d-lg-none'>
            <img src={exitImage} alt='Exit' className='log-out-icon' />
          </button>
          <button onClick={exit} type='button' className='log-out-text d-none d-lg-flex'>Log out</button>
        </div>
      </div>
    </div>
  )
}

export default UpBar