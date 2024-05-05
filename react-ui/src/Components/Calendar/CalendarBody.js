import './CalendarBody.css';
import clsx from 'clsx';
import useCalendar from '../../hooks/useCalendar';
import useGoal from "../../hooks/useGoal";
import { getLastMonthDates, getDatesInMonth, getNextMonthDates, getLastDayInMonth, getWeekDay, getNewDateNoTime } from '../../Services/Calendar/calendarService';

const CalendarBody = () => {
  const {currentDateArea} = useCalendar();

  const {dates, activeGoal} = useGoal();

  const weekDaysNames = ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'];
  const statelessColor = '#ADADAD';

  const firstWeekDay = getWeekDay(currentDateArea, 1);
  const lastWeekDay = getWeekDay(getLastDayInMonth(currentDateArea), 1);

  const lastMonthDates = getLastMonthDates(currentDateArea, firstWeekDay);
  const currentMonthDates = getDatesInMonth(currentDateArea);
  const nextMonthDates = getNextMonthDates(currentDateArea, 6 - lastWeekDay)

  const cells = lastMonthDates.concat(currentMonthDates, nextMonthDates);

  const spanFromDate = (state = null, date) => {
    const startDate = getNewDateNoTime(activeGoal.dateStart);
    const goalEnd = getNewDateNoTime(activeGoal.dateStart);
    goalEnd.setDate(startDate.getDate() + activeGoal.daysNumber);
    const now = getNewDateNoTime();
    const endDay = now <= goalEnd ? now : goalEnd;

    let statusEl =
    <span
      style={state ? {backgroundColor: state.color} : {backgroundColor: statelessColor}}
      className={clsx(startDate <= date && date <= endDay && 'cell-status')}
    >
    </span>
    return statusEl
  }

  const createSpan = (dateShift, ind) => {
    let statusEl;
    if (activeGoal && dates && dates.length > 0){
      if (dateShift < dates.length && cells[ind].getTime() === getNewDateNoTime(dates[dateShift].date).getTime()){
        let status = activeGoal.template.states.find(state => state.id === dates[dateShift].stateId)
        statusEl = spanFromDate(status, cells[ind])
        dateShift++;
      }
      else {
        statusEl = spanFromDate(null, cells[ind])
      }
    }
    return statusEl;
  }


  const cellsElements = cells => {
    let content = [];
    let dateShift = 0;
    for (let i = 0; i < cells.length; i++){
      let dateEl = cells[i].getDate();
      let statusEl = createSpan(dateShift, i);

      let item =
      <div key={cells[i]} className='calendar-cell'>
        {i < 7 && <span>{weekDaysNames[i]}</span>}

        {dateEl === 1 ? 
        <span>{dateEl + ' ' + cells[i].toLocaleString('en-us', { month: 'short'})}</span> :
        <span>{dateEl}</span>
        }

        {statusEl ?? statusEl}
      </div>

      content.push(item);
    }
    return content;
  };

  return (
    <div className={clsx('calendar-body', cells.length <= 35 && 'grid-35', cells.length > 35 && 'grid-42')}>
      {cellsElements(cells)}
    </div>
  )
}

export default CalendarBody