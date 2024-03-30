import './Calendar.css';
import useCalendar from '../../hooks/useCalendar';
import { getLastMonthDates } from '../../Services/Calendar/calendarService';

const CalendarBody = () => {
  const {currentDateArea} = useCalendar();

  const weekDaysNames = ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'];

  const firstWeekDay = (currentDateArea.getDay() + 6) % 7;
  const lastMonthDays = getLastMonthDates(currentDateArea, firstWeekDay);
  console.log(lastMonthDays);
  return (
    <div className='calendar-body'>
      {weekDaysNames.map(dateName => <div key={dateName}>{dateName}</div>)}
    </div>
  )
}

export default CalendarBody