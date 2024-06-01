import CalendarBody from '../Calendar/CalendarBody';
import DateModal from './DateModal';
import './Home.css';
import SideBar from "./SideBar";
import UpBar from './UpBar';


const Home = () => {

  return (

    <div className='container-fluid home'>
      <UpBar />
      <div className='row no-gutters flex-grow-1 calendar-window'>
        <div className='col-lg-2 p-0'>
          <SideBar />
        </div>
        <div className='col p-0'>
          <CalendarBody />
        </div>
      </div>
      <DateModal></DateModal>
    </div>
  )
}

export default Home