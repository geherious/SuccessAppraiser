import './Home.css';
import SideBar from "./SideBar";
import { GoalProvider } from '../../Context/GoalProvide';
import UpBar from './UpBar';
import CalendarBody from '../Calendar/CalendarBody';
import { CalendarProvider } from '../../Context/CalendarProvide';
import { useRef, useState } from 'react';
import DateModal from './DateModal';


const Home = () => {

  const [isActive, setIsActive] = useState(false);
  const [modalDate, setModalDate] = useState(new Date());
  return (
    <CalendarProvider>
    <GoalProvider>
      <div className='container-fluid home'>
        <UpBar/>
        <div className='row no-gutters flex-grow-1 calendar-window'>
          <div className='col-lg-2 p-0'>
            <SideBar/>
          </div>
          <div className='col p-0'>
            <CalendarBody setIsActive={setIsActive} setModalDate={setModalDate}/>
          </div>
        </div>
        <DateModal isActive={isActive} setIsActive={setIsActive} date={modalDate}></DateModal>
      </div>
    </GoalProvider>
    </CalendarProvider>
  )
}

export default Home