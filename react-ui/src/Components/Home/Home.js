import './Home.css';
import SideBar from "./SideBar";
import useAuth from "../../hooks/useAuth";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import useGoal from "../../hooks/useGoal";
import { GoalProvider } from '../../Context/GoalProvide';


const Home = () => {

  return (
    <GoalProvider>
      <div className='container-fluid home'>
        <div className="row vh-100 no-gutters">
        <SideBar/>
        <div className="col calendar">
          asd
        </div>
        </div>
      </div>
    </GoalProvider>
  )
}

export default Home