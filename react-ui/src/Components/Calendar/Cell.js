import './Cell.css';
import useGoal from "../../hooks/useGoal";
import clsx from 'clsx';
import { getStartAndEndDate } from '../../Services/Calendar/calendarService';
import { memo, useRef } from 'react';
import useHomeStore from '../../Store/useHomeStore';
import useDates from '../../hooks/useDates';


const Cell = ({ date, dateState, cellNumber }) => {
  const activeGoal = useHomeStore(state => state.activeGoal);
  const setModalIsActive = useHomeStore(state => state.setModalIsActive);
  const setModalDate = useHomeStore(state => state.setModalDate);

  const weekDaysNames = ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'];
  const statelessColor = '#ADADAD';

  const createStatusSpan = () => {
    let statusEl;
    if (!dateState.type) {
      return statusEl;
    }
    statusEl =
      <span
        style={dateState.state ? { backgroundColor: dateState.state.color } : { backgroundColor: statelessColor }}
        className={'cell-status'}
        onClick={() => {
          if (!state) {
            setModalIsActive(true);
            setModalDate(date);
          }
        }}
      >
      </span>
    return statusEl
  }
  const dateEl = date.getDate();
  const statusEl = createStatusSpan();

  return (
    <div className='calendar-cell'>
      {cellNumber < 7 && <span>{weekDaysNames[cellNumber]}</span>}

      {dateEl === 1 ?
        <span>{dateEl + ' ' + date.toLocaleString('en-us', { month: 'short' })}</span> :
        <span>{dateEl}</span>
      }
      {statusEl}
    </div>
  )
};

export default Cell