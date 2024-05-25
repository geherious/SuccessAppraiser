import './Home.css';
import SideBar from "./SideBar";
import { GoalProvider } from '../../Context/GoalProvide';
import UpBar from './UpBar';
import CalendarBody from '../Calendar/CalendarBody';
import { CalendarProvider } from '../../Context/CalendarProvide';
import { useRef, useState } from 'react';
import DateModal from './DateModal';
import useHomeStore from '../../Store/useHomeStore';


const Home = () => {

  return (

      <div className='container-fluid home'>
        <UpBar/>
        <div className='row no-gutters flex-grow-1 calendar-window'>
          <div className='col-lg-2 p-0'>
            <SideBar/>
          </div>
          <div className='col p-0'>
            <CalendarBody/>
          </div>
        </div>
        <DateModal></DateModal>
      </div>
  )
}

export default Home