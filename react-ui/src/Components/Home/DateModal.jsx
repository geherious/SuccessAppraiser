import { useEffect, useState } from 'react';
import { getDateOnlyString } from '../../Services/Calendar/calendarService';
import useHomeStore from '../../Store/useHomeStore';
import { goalDateEndpoint } from '../../api/goalApi';
import LoaderDots from '../Loaders/LoaderDots';
import ModalBase from '../ModalBase/ModalBase';
import './DateModal.css';
import useDates from '../../hooks/useDates';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import useAxiosPrivate from '../../hooks/useAxiosPrivate';


const DateModal = () => {
  const activeGoal = useHomeStore(state => state.activeGoal);
  const [status, setStatus] = useState('');
  const [comment, setComment] = useState('');
  const [existingDate, setExistingDate] = useState(null);

  const isActive = useHomeStore(state => state.dateModalIsActive);
  const setIsActive = useHomeStore(state => state.setDateModalIsActive);
  const [isLoading, setIsLoading] = useState(false);

  const date = useHomeStore(state => state.dateModalDate);
  const { dates, mutate } = useDates();

  const axiosPrivate = useAxiosPrivate();

  useEffect(() => {
    if (activeGoal) {
      setStatus(activeGoal.template.states[0].id);
    }
  }, [activeGoal])

  useEffect(() => {
    if (dates){
      let savedDate = dates.find(d => d.date === getDateOnlyString(date));
      if (savedDate){
        setExistingDate(savedDate);
        setStatus(savedDate.stateId);
        setComment(savedDate.comment);
      }
      else{
        setExistingDate(null);
        setComment("");
        setStatus(activeGoal.template.states[0].id);
      }
    }
  }, [date, isActive])

  const submitForm = async (e) => {
    setIsLoading(true);
    e.preventDefault();
    const newData = { date: getDateOnlyString(date), comment: comment, stateId: status}
    try{
      if (existingDate){
        await axiosPrivate.put(goalDateEndpoint(activeGoal.id), JSON.stringify(newData));
      }
      else{
        await axiosPrivate.post(goalDateEndpoint(activeGoal.id), JSON.stringify(newData));
      }
      mutate(date, newData);
    }
    catch (error) {
      console.log(error);
      toast.error('Something went wrong');
      return;
    }
    finally{
      setIsLoading(false);
      setIsActive(false);
    }
  }

  return (
    <ModalBase isActive={isActive} setIsActive={setIsActive}>
      {activeGoal ?
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
            <button type="submit" disabled={isLoading}>Save</button>
            <button type="button" onClick={() => setIsActive(false)}>Cancel</button>
          </div>
        </form>
        ) :
        (<LoaderDots />)
      }
    </ModalBase>
  )
}

export default DateModal