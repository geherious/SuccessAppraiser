import useHomeStore from '../../Store/useHomeStore';
import './Cell.css';


const Cell = ({ date, dateState, cellNumber }) => {
  const setModalIsActive = useHomeStore(state => state.setDateModalIsActive);
  const setModalDate = useHomeStore(state => state.setDateModalDate);

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
          setModalDate(date);
          setModalIsActive(true);
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