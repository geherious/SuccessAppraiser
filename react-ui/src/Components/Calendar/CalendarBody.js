import './CalendarBody.css';
import clsx from 'clsx';
import useCalendar from '../../hooks/useCalendar';
import { getLastMonthDates, getDatesInMonth, getNextMonthDates, getLastDayInMonth, getWeekDay } from '../../Services/Calendar/calendarService';
import Cell from './Cell';

const CalendarBody = () => {
  const { currentDateArea } = useCalendar();

  const firstWeekDay = getWeekDay(currentDateArea, 1);
  const lastWeekDay = getWeekDay(getLastDayInMonth(currentDateArea), 1);

  const lastMonthDates = getLastMonthDates(currentDateArea, firstWeekDay);
  const currentMonthDates = getDatesInMonth(currentDateArea);
  const nextMonthDates = getNextMonthDates(currentDateArea, 6 - lastWeekDay)

  const cells = lastMonthDates.concat(currentMonthDates, nextMonthDates);


  const cellsElements = cells => {
    let content = [];
    let dateShift = 0;
    for (let i = 0; i < cells.length; i++) {
      const date = cells[i];
      const item = <Cell key={date} date={date} dateShift={dateShift} cellNumber={i}/>;

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