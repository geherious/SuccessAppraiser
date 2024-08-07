import './SideBar.css';
import plusImg from '../../assets/images/home/plus-icon.png';
import clsx from 'clsx';
import LoaderDots from '../Loaders/LoaderDots';
import useNewGoal from '../../hooks/useNewGoal';
import useHomeStore from '../../Store/useHomeStore';


const SideBar = () => {

  const { goals, IsLoadingGoals } = useNewGoal();
  const activeGoal = useHomeStore(state => state.activeGoal);
  const setActiveGoal = useHomeStore(state => state.setActiveGoal);
  const setGoalModalIsActive = useHomeStore(state => state.setGoalModalIsActive);

  const openGoalModal = () => {
    setGoalModalIsActive(true);
  }

  let content;

  if (goals) {
    content =
      <>
      <div className='add-button-container'>
        <button onClick={openGoalModal} type='button' className='add-button'>
          <img src={plusImg} alt='Add' className='add-icon' />
        </button>
      </div>
        <ul className='goal-list'>
          {goals.map(goal =>
            <li key={goal.id} onClick={() => setActiveGoal(goal)} className={clsx(goal.id === activeGoal?.id && 'active-goal')}>{goal.name}</li>
          )}
        </ul>
      </>
  }
  else {
    content = <LoaderDots />
  }
  return (
    <div className='side-bar'>
      {content}
    </div>
  )
}

export default SideBar