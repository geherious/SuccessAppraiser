import clsx from 'clsx';
import { compareDateOnly, getDatesInMonth, getLastDayInMonth, getLastMonthDates, getNextMonthDates, getStartAndEndDate, getWeekDay } from '../../Services/Calendar/calendarService';
import useHomeStore from '../../Store/useHomeStore';
import useDates from '../../hooks/useDates';
import './CalendarBody.css';
import Cell from './Cell';

const CalendarBody = () => {
  const currentDateArea = useHomeStore(state => state.currentDateArea);
  const { dates } = useDates();
  const activeGoal = useHomeStore(state => state.activeGoal);

  const firstWeekDay = getWeekDay(currentDateArea, 1);
  const lastWeekDay = getWeekDay(getLastDayInMonth(currentDateArea), 1);

  const lastMonthDates = getLastMonthDates(currentDateArea, firstWeekDay);
  const currentMonthDates = getDatesInMonth(currentDateArea);
  const nextMonthDates = getNextMonthDates(currentDateArea, 6 - lastWeekDay)

  const cells = lastMonthDates.concat(currentMonthDates, nextMonthDates);

  const findDateState = (date, dateShift) => {
    const result = {
      type: null,
      state: null
    }

    let isInRange = false;

    if (!activeGoal || !dates) {
      return result;
    }

    const { startDate, endDate } = getStartAndEndDate(new Date(activeGoal.dateStart), activeGoal.daysNumber);

    if (date.getTime() >= startDate.getTime() && date.getTime() <= endDate.getTime()) {
      isInRange = true;
    }

    if (!isInRange) {
      return result;
    }

    if (isInRange && (dates.length < 0 || dateShift.value >= dates.length)) {
      result.type = 'stateless';
      return result;
    }

    while (dateShift.value < dates.length - 1 && compareDateOnly(dates[dateShift.value].date, date) < 0) {
      dateShift.value++;
    }
    if (compareDateOnly(date, dates[dateShift.value].date) === 0) {
      const state = activeGoal.template.states.find(state => state.id === dates[dateShift.value].stateId)
      result.type = 'stateful';
      result.state = state;
    }
    else {
      result.type = 'stateless';
    }

    return result;
  };



  const cellsElements = cells => {
    let content = [];
    let dateShift = { value: 0 };
    for (let i = 0; i < cells.length; i++) {
      const date = cells[i];
      const dateState = findDateState(date, dateShift);
      const item = <Cell key={date} date={date} dateState={dateState} cellNumber={i} />;


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