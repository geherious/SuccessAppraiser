import './CalendarBody.css';
import clsx from 'clsx';
import useCalendar from '../../hooks/useCalendar';
import { getLastMonthDates, compareDateOnly, getDatesInMonth, getNextMonthDates, getLastDayInMonth, getWeekDay } from '../../Services/Calendar/calendarService';
import Cell from './Cell';
import useGoal from '../../hooks/useGoal';

const CalendarBody = ({ setIsActive, setModalDate}) => {
  const { currentDateArea } = useCalendar();
  const { dates } = useGoal();

  const firstWeekDay = getWeekDay(currentDateArea, 1);
  const lastWeekDay = getWeekDay(getLastDayInMonth(currentDateArea), 1);

  const lastMonthDates = getLastMonthDates(currentDateArea, firstWeekDay);
  const currentMonthDates = getDatesInMonth(currentDateArea);
  const nextMonthDates = getNextMonthDates(currentDateArea, 6 - lastWeekDay)

  const cells = lastMonthDates.concat(currentMonthDates, nextMonthDates);

  const isDateWithState = (date, dateShift) => {
    if (dates && dates.length > 0 && dateShift < dates.length && compareDateOnly(date, dates[dateShift].date)) {
      return true;
    }
    else{
      return false;
    }
  };


  const cellsElements = cells => {
    let content = [];
    let dateShift = 0;
    for (let i = 0; i < cells.length; i++) {
      const date = cells[i];
      const isStateful = isDateWithState(date, dateShift);
      if (dates){
        console.log(date, isStateful, dateShift);
      }
      const item = <Cell key={date} date={date} dateShift={dateShift} isDateWithState={isStateful} cellNumber={i} setIsActive={setIsActive} setModalDate={setModalDate}/>;

      if (isStateful){
        dateShift++;
      }

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