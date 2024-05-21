import './Cell.css';
import useGoal from "../../hooks/useGoal";
import clsx from 'clsx';
import { getNewDateNoTime } from '../../Services/Calendar/calendarService';
import { useRef } from 'react';


const Cell = ({ date, dateShift, cellNumber }) => {

    const {activeGoal, dates} = useGoal();

    const weekDaysNames = ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'];
    const statelessColor = '#ADADAD';

    const spanFactory = (state = null) => {
        const startDate = getNewDateNoTime(activeGoal.dateStart);
        const goalEnd = getNewDateNoTime(activeGoal.dateStart);
        goalEnd.setDate(startDate.getDate() + activeGoal.daysNumber);
        const now = getNewDateNoTime();
        const endDay = now <= goalEnd ? now : goalEnd;

        let statusEl =
            <span
                style={state ? { backgroundColor: state.color } : { backgroundColor: statelessColor }}
                className={clsx(startDate <= date && date <= endDay && 'cell-status')}
            >
            </span>
        return statusEl
    }

    const createStatusSpan = () => {
        let statusEl;
        if (activeGoal && dates) {
            if (dates.length > 0 && dateShift < dates.length && date.getTime() === getNewDateNoTime(dates[dateShift].date).getTime()) {
                let status = activeGoal.template.states.find(state => state.id === dates[dateShift].stateId)
                statusEl = spanFactory(status)
                dateShift++;
            }
            else {
                statusEl = spanFactory(null)
            }
        }
        return statusEl;
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

            {statusEl ?? statusEl}
        </div>
    )
}

export default Cell