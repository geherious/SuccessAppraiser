import './CalendarBody.css';
import clsx from 'clsx';
import useCalendar from '../../hooks/useCalendar';
import { getLastMonthDates, getDatesInMonth, getNextMonthDates, getLastDayInMonth, getWeekDay } from '../../Services/Calendar/calendarService';

const CalendarBody = () => {
  const {currentDateArea} = useCalendar();
  
  const weekDaysNames = ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'];

  const firstWeekDay = getWeekDay(currentDateArea, 1);
  const lastWeekDay = getWeekDay(getLastDayInMonth(currentDateArea), 1);

  const lastMonthDates = getLastMonthDates(currentDateArea, firstWeekDay);
  const currentMonthDates = getDatesInMonth(currentDateArea);
  const nextMonthDates = getNextMonthDates(currentDateArea, 6 - lastWeekDay)

  const cells = lastMonthDates.concat(currentMonthDates, nextMonthDates);

  const cellsElements = cells => {
    let content = [];
    for (let i = 0; i < cells.length; i++){
      let item =
      <div key={cells[i]} className='calendar-cell'>
        {i < 7 && <span>{weekDaysNames[i]}</span>}
        {cells[i].getDate() === 1 ? 
        <span>{cells[i].getDate() + ' ' + cells[i].toLocaleString('en-us', { month: 'short'})}</span> :
        <span>{cells[i].getDate()}</span>
      }
      <span className='calendar-cell-skill'>asd</span>
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