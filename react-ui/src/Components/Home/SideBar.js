import './SideBar.css';
import plusImg from '../../../public/images/home/plus.png';
import clsx from 'clsx';
import LoaderDots from '../Loaders/LoaderDots';
import useNewGoal from '../../hooks/useNewGoal';
import useHomeStore from '../../Store/useHomeStore';


const SideBar = () => {

  const { goals, IsLoadingGoals } = useNewGoal();
  const activeGoal = useHomeStore(state => state.activeGoal);
  const setActiveGoal = useHomeStore(state => state.setActiveGoal);

  let content;

  if (goals) {
    content =
      <>
        <button type='button' className='add-button'>
          <img src={plusImg} alt='Add' className='add-icon' />
        </button>
        <ul className='goal-list'>
          {goals.map(goal =>
            <li key={goal.id} onClick={() => setActiveGoal(goal)} className={clsx(goal === activeGoal && 'active-goal')}>{goal.name}</li>
          )}
        </ul>
      </>
  }
  else if (IsLoadingGoals) {
    content = <LoaderDots />
  }
  else {
    content = <p>Error</p>
  }
  return (
    <div className='side-bar'>
      {content}
    </div>
  )
}

export default SideBar