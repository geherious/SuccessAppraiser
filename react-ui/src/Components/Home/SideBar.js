import './SideBar.css';
import PlusImg from '../../../public/images/plus.png';
import useGoal from '../../hooks/useGoal';


const SideBar = () => {

  const { goals, goalIsLoading } = useGoal();
  let content;

  if (goals){
    content = 
    <>
        <button type='button' className='add-button'>
        <img src={PlusImg} alt='Add' className='add-icon'/>
        </button>
        <ul>
          {goals.data.map(goal => <li key={goal.id}>{goal.name}</li>)}
        </ul>
    </>
  }
  else if (goalIsLoading){
    content = <p>Loading</p>
  }
  else{
    content = <p>Error</p>
  }
  return (
    <div className='col-md-2 side-bar'>
        {content}
    </div>
  )
}

export default SideBar