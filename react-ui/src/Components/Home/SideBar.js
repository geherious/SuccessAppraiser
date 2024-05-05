import './SideBar.css';
import plusImg from '../../../public/images/home/plus.png';
import useGoal from '../../hooks/useGoal';
import clsx from 'clsx';
import LoaderDots from '../Loaders/LoaderDots';


const SideBar = () => {

  const { goals, goalIsLoading, activeGoal, setActiveGoal } = useGoal();
  let content;

  if (goals){
    content = 
    <>
        <button type='button' className='add-button'>
        <img src={plusImg} alt='Add' className='add-icon'/>
        </button>
        <ul className='goal-list'>
          {goals.map(goal =>
          <li key={goal.id} onClick={() => setActiveGoal(goal)} className={clsx(goal === activeGoal && 'active-goal')}>{goal.name}</li>
          )}
        </ul>
    </>
  }
  else if (goalIsLoading){
    content = <LoaderDots/>
  }
  else{
    content = <p>Error</p>
  }
  return (
    <div className='side-bar'>
        {content}
    </div>
  )
}

export default SideBar