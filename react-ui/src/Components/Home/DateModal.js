import ModalBase from '../ModalBase/ModalBase';
import './DateModal.css';
import useGoal from "../../hooks/useGoal";


const DateModal = ({isActive, setIsActive, date}) => {

  const {activeGoal} = useGoal();

  return (
    <ModalBase isActive={isActive} setIsActive={setIsActive}>
      <form>
        <div className='date-modal-header'>
            <span className='date'>{new Date().toLocaleDateString('en-us', {day:'numeric', month: 'long'})}</span>
        </div>
        <div className='date-modal-body'>
          <label htmlFor='modal-status-select'>Your day result</label>
          <select id='modal-status-select'>
            {activeGoal && activeGoal.template.states.map(state => (
              <option key={state.id} value={state.id}>{state.name}</option>
            ))}
          </select>

          <label htmlFor='modal-comment-input'>Comment</label>
          <textarea id='modal-comment-input' rows={4}/>
        </div>
        <div className='date-modal-footer'>

        </div>
      </form>
    </ModalBase>
  )
}

export default DateModal