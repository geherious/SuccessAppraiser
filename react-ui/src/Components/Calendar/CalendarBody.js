import './CalendarBody.css';
import clsx from 'clsx';
import useCalendar from '../../hooks/useCalendar';
import { getLastMonthDates, compareDateOnly, getDatesInMonth, getNextMonthDates, getLastDayInMonth, getWeekDay } from '../../Services/Calendar/calendarService';
import Cell from './Cell';
import useGoal from '../../hooks/useGoal';
import useHomeStore from '../../Store/useHomeStore';
import useDates from '../../hooks/useDates';
import { useCallback } from 'react';

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
    if (dates && dates.length > 0 && dateShift.value < dates.length) {
      while (dateShift.value < dates.length - 1 && compareDateOnly(dates[dateShift.value].date, date) < 0){
        dateShift.value++;
      }
      if (compareDateOnly(date, dates[dateShift.value].date) === 0){
        const result = activeGoal.template.states.find(state => state.id === dates[dateShift.value].stateId)
        dateShift.value++;
        return result
      }
      else{
        return null;
      }
    }
    else{
      return null;
    }
  };



  const cellsElements = cells => {
    let content = [];
    let dateShift = { value: 0};
    for (let i = 0; i < cells.length; i++) {
      const date = cells[i];
      const state = findDateState(date, dateShift);
      const item = <Cell key={date} date={date} state={state} cellNumber={i}/>;


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