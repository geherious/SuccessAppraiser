import ModalBase from '../ModalBase/ModalBase';
import './DateModal.css';
import useGoal from "../../hooks/useGoal";
import { useEffect, useState } from 'react';
import LoaderDots from '../Loaders/LoaderDots';
import useAxiosPrivate from '../../hooks/useAxiosPrivate';
import { postGoalDate } from '../../api/goalApi';
import { getDateOnlyString } from '../../Services/Calendar/calendarService';


const DateModal = ({isActive, setIsActive, date}) => {

  const {activeGoal} = useGoal();

  const [status, setStatus] = useState('');
  const [comment, setComment] = useState('');

  const { isConfiguring, axiosPrivate } = useAxiosPrivate();

  useEffect(() => {
    if (activeGoal){
      setStatus(activeGoal.template.states[0].id);
    }
  }, [activeGoal])

  const submitForm = (e) => {
    e.preventDefault();
    try{
      const response = axiosPrivate.post(postGoalDate, JSON.stringify({
        date: getDateOnlyString(date), comment: comment, stateId: status, goalId: activeGoal.id}));
      setIsActive(false);
    } catch (err) {
      console.log(err);
    }
  }

  return (
    <ModalBase isActive={isActive} setIsActive={setIsActive}>
      {activeGoal && !isConfiguring ? 
      (<form onSubmit={submitForm}>
        <div className='date-modal-header'>
            <span className='date'>{date.toLocaleDateString('en-us', {day:'numeric', month: 'long'})}</span>
        </div>

        <div className='date-modal-body'>
          <label htmlFor='date-modal-status-select'>Your day result</label>
          <select id='date-modal-status-select' value={status} onChange={e => setStatus(e.target.value)}>
            {activeGoal.template.states.map(state => (
              <option key={state.id} value={state.id}>{state.name}</option>
            ))}
          </select>
          <label htmlFor='date-modal-comment-input'>Comment</label>
          <textarea id='date-modal-comment-input' value={comment} onChange={e => setComment(e.target.value)} rows={4}/>
        </div>

        <div className="date-modal-footer">
          <button type="submit">Save</button>
          <button type="button" onClick={() => setIsActive(false)}>Cancel</button>
        </div>
        <div className='date-modal-footer'>

        </div>
      </form>
      ) : 
      (<LoaderDots/>)
      }
    </ModalBase>
  )
}

export default DateModal