import { useEffect, useState } from 'react';
import { getDateOnlyString } from '../../Services/Calendar/calendarService';
import useHomeStore from '../../Store/useHomeStore';
import { postGoalDate } from '../../api/goalApi';
import useAxiosPrivate from '../../hooks/useAxiosPrivate';
import LoaderDots from '../Loaders/LoaderDots';
import ModalBase from '../ModalBase/ModalBase';
import './DateModal.css';
import useDates from '../../hooks/useDates';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


const DateModal = () => {
  const activeGoal = useHomeStore(state => state.activeGoal);
  const [status, setStatus] = useState('');
  const [comment, setComment] = useState('');

  const isActive = useHomeStore(state => state.modalIsActive);
  const setIsActive = useHomeStore(state => state.setModalIsActive);
  const date = useHomeStore(state => state.modalDate);
  const { mutate } = useDates();

  const { isConfiguring, axiosPrivate } = useAxiosPrivate();

  useEffect(() => {
    if (activeGoal) {
      setStatus(activeGoal.template.states[0].id);
    }
  }, [activeGoal])

  const submitForm = async (e) => {
    e.preventDefault();
    const newData = { date: getDateOnlyString(date), comment: comment, stateId: status, goalId: activeGoal.id }
    try {
      await axiosPrivate.post(postGoalDate, JSON.stringify(newData));
      delete newData.goalId;
      mutate(date, newData);
      setIsActive(false);
    } catch (error) {
      console.log(error);
      setIsActive(false);
      toast.error('Something went wrong');

    }
  }

  return (
    <ModalBase isActive={isActive} setIsActive={setIsActive}>
      {activeGoal && !isConfiguring ?
        (<form onSubmit={submitForm}>
          <div className='date-modal-header'>
            <span className='date'>{date.toLocaleDateString('en-us', { day: 'numeric', month: 'long' })}</span>
          </div>

          <div className='date-modal-body'>
            <label htmlFor='date-modal-status-select'>Your day result</label>
            <select id='date-modal-status-select' value={status} onChange={e => setStatus(e.target.value)}>
              {activeGoal.template.states.map(state => (
                <option key={state.id} value={state.id}>{state.name}</option>
              ))}
            </select>
            <label htmlFor='date-modal-comment-input'>Comment</label>
            <textarea id='date-modal-comment-input' value={comment} onChange={e => setComment(e.target.value)} rows={4} />
          </div>

          <div className="date-modal-footer">
            <button type="submit">Save</button>
            <button type="button" onClick={() => setIsActive(false)}>Cancel</button>
          </div>
          <div className='date-modal-footer'>

          </div>
        </form>
        ) :
        (<LoaderDots />)
      }
    </ModalBase>
  )
}

export default DateModal