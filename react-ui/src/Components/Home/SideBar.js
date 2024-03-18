import './SideBar.css';
import PlusImg from '../../../public/images/plus.png'


const SideBar = ({goals, goalError, goalMutate}) => {
  // const goalList = goals.map(goal => <li>{goal}</li>)

  return (
    <div className='col-md-2 side-bar'>
        <button type='button' className='add-button'>
            <img src={PlusImg} alt='Add' className='add-icon'/>
        </button>
        <ul>
          
        </ul>
    </div>
  )
}

export default SideBar