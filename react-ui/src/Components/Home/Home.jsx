import { useRef } from 'react';
import CalendarBody from '../Calendar/CalendarBody';
import DateModal from './DateModal';
import './Home.css';
import SideBar from "./SideBar";
import UpBar from './UpBar';
import GoalModal from './GoalModal';


const Home = () => {

  const sideBarRefCol = useRef(null);

  return (

    <div className='container-fluid home'>
      <UpBar sideBarRefCol={sideBarRefCol}/>
      <div className='row no-gutters content flex-grow-1 overflow-hidden'>
        <div className='col-lg-2 h-100 d-flex p-0 hidden' ref={sideBarRefCol}>
          <SideBar />
        </div>
        <div className='col d-flex p-0'>
          <CalendarBody />
        </div>
      </div>
      <DateModal/>
      <GoalModal/>
    </div>
  )
}

export default Home