import './Home.css';
import SideBar from "./SideBar";
import useAuth from "../../hooks/useAuth";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import useGoal from "../../hooks/useGoal";
import { GoalProvider } from '../../Context/GoalProvide';
import UpBar from './UpBar';
import CalendarBody from '../Calendar/CalendarBody';
import { CalendarProvider } from '../../Context/CalendarProvide';


const Home = () => {
  return (
    <GoalProvider>
    <CalendarProvider>
      <div className='container-fluid home'>
        <UpBar/>
        <div className='row no-gutters flex-grow-1 calendar-window'>
          <div className='col-md-2 p-0'>
            <SideBar/>
          </div>
          <div className='col p-0'>
            <CalendarBody/>
          </div>
        </div>
      </div>
    </CalendarProvider>
    </GoalProvider>
  )
}

export default Home