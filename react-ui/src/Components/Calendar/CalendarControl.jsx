import arrowImage from "../../assets/images/home/arrow-icon.png";
import useHomeStore from "../../Store/useHomeStore";
import './CalendarControl.css';

const CalendarControl = () => {

  const currentDateArea = useHomeStore(state => state.currentDateArea);
  const setCurrentDateArea = useHomeStore(state => state.setCurrentDateArea);

  const shiftMonth = (value) => {
    setCurrentDateArea(new Date(currentDateArea.setMonth(currentDateArea.getMonth() + value)));
  }

  return (
    <div className="calendar-control">
      <button onClick={() => shiftMonth(-1)} className="control-button"><img className="control-image reverse-image" src={arrowImage} alt='prev' /></button>
      <button onClick={() => shiftMonth(1)} className="control-button"><img className="control-image" src={arrowImage} alt='next' /></button>
      <span className="date">{currentDateArea.toLocaleDateString('en-us', { month: 'long', year: 'numeric' })}</span>
    </div>
  )
}

export default CalendarControl