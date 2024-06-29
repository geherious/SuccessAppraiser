import CalendarBody from '../Calendar/CalendarBody';
import DateModal from './DateModal';
import './Home.css';
import SideBar from "./SideBar";
import UpBar from './UpBar';


const Home = () => {

  return (

    <div className='container-fluid home'>
      <UpBar />
      <div className='row no-gutters content flex-grow-1 overflow-hidden'>
        <div className='col-lg-2 h-100 d-flex p-0'>
          <SideBar />
        </div>
        <div className='col d-flex p-0'>
          <CalendarBody />
        </div>
      </div>
      <DateModal/>
    </div>
  )
}

export default Home