import { Link, useNavigate } from "react-router-dom"
import useAxiosPrivate from "../../hooks/useAxiosPrivate"
import useLogout from "../../hooks/useLogout";
import { getGoals, goalsUrlEndpoint } from "../../api/GoalApi";
import './Home.css';
import SideBar from "./SideBar";
import useSWR from "swr";
import { useEffect } from "react";


const Home = () => {
  const axiosPrivate = useAxiosPrivate()

  useEffect(() => {
    const rs = async () => {
      return await axiosPrivate.get(goalsUrlEndpoint)
    }
    const response = rs();
    console.log(response);
  }, [])

  const {
    data: goals,
    error: goalError,
    mutate: goalMutate,
    isLoading: goalIsLoading
  } = useSWR(goalsUrlEndpoint, () =>axiosPrivate.get(goalsUrlEndpoint));
  let content;

  if (goalIsLoading || (goalError && goalError.response.status === 401)){
    content = <p>Loading</p>
  }
  else if (goalError){
    content = <p>Error</p>
  }
  else {
    content =
    <div className="row vh-100 no-gutters">
    <SideBar
    goals={goals}
    goalError={goalError}
    goalMutate={goalMutate}
    />
    <div className="col calendar">
      asd
    </div>
    </div>
  }
  return (
    <div className='container-fluid home'>
      {content}
    </div>
  )
}

export default Home