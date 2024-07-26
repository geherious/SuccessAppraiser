import CalendarControl from '../Calendar/CalendarControl';
import './UpBar.css';
import sideBarImage from '../../assets/images/home/hamburger-sidebar.svg';
const UpBar = ({sideBarRefCol}) => {

  const toggleSideBar = () => {
    if (sideBarRefCol.current.classList.contains('hidden')) {
      sideBarRefCol.current.classList.remove('hidden');
    }
    else {
      sideBarRefCol.current.classList.add('hidden');
    }
  }

  return (
    <div className='row no-gutters'>
      <div className='col-lg-2 d-none d-lg-flex p-0'>
        <span className='logo-span'>
          Suap?
        </span>
      </div>
      <div className='col p-0 d-flex'>
        <div className='side-button-container d-lg-none'>
          <button onClick={toggleSideBar} type='button' className='side-button'>
            <img src={sideBarImage} alt='Toggle' className='side-icon' />
          </button>
        </div>
        <CalendarControl />
      </div>
    </div>
  )
}

export default UpBar