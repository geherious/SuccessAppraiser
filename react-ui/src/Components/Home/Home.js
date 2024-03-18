import { Link, useNavigate } from "react-router-dom"
// import useAxiosPrivate from "../../hooks/useAxiosPrivate"
import useLogout from "../../hooks/useLogout";
import { getGoals, goalsUrlEndpoint } from "../../api/GoalApi";
import './Home.css';
import SideBar from "./SideBar";
import useSWR from "swr";
import { useEffect } from "react";
import useAuth from "../../hooks/useAuth";
import useRefreshToken from "../../hooks/useRefreshToken";
import Dummy from "../Dummy"


const Home = () => {
  // const axiosPrivate = useAxiosPrivate()
  const {auth} = useAuth();

  // const {
  //   data: goals,
  //   error: goalError,
  //   mutate: goalMutate,
  //   isLoading: goalIsLoading
  // } = useSWR(goalsUrlEndpoint, () =>axiosPrivate.get(goalsUrlEndpoint));
  // let content;


  // if (goalIsLoading || (goalError && goalError.response.status === 401)){
  //   content = <p>Loading</p>
  // }
  // else if (goalError){
  //   content = <p>Error</p>
  // }
  // else {
  //   content =
  //   <div className="row vh-100 no-gutters">
  //   <SideBar
  //   goals={goals}
  //   goalError={goalError}
  //   goalMutate={goalMutate}
  //   />
  //   <div className="col calendar">
  //     asd
  //   </div>
  //   </div>
  // }
  return (
    <div className='container-fluid home'>
      <Dummy/>
    </div>
  )
}

export default Home