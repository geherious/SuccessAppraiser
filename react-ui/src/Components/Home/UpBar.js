import CalendarControl from '../Calendar/CalendarControl';
import './UpBar.css';
const UpBar = () => {
  return (
    <div className='row no-gutters'>
      <div className='col-md-2 p-0'>
        <span className='logo-span'>
          Suap?
        </span>
      </div>
      <div className='col p-0'>
        <CalendarControl/>
      </div>
  </div>
  )
}

export default UpBar