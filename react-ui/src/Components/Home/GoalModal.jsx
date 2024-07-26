import { useEffect, useRef, useState } from 'react';
import useHomeStore from '../../Store/useHomeStore';
import ModalBase from '../ModalBase/ModalBase';
import './DateModal.css';
import 'react-toastify/dist/ReactToastify.css';
import useAxiosPrivate from '../../hooks/useAxiosPrivate';
import { getDateOnlyString } from '../../Services/Calendar/calendarService';
import { goalsUrlEndpoint } from '../../api/goalApi';
import { toast } from 'react-toastify';
import useNewGoal from '../../hooks/useNewGoal';


const GoalModal = () => {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [numberOfDays, setNumberOfDays] = useState(21);
  const [startDate, setStartDate] = useState(getDateOnlyString(new Date()));

  const isActive = useHomeStore(state => state.goalModalIsActive);
  const setIsActive = useHomeStore(state => state.setGoalModalIsActive);

  const [isLoading, setIsloading] = useState(false);
  const submitButton = useRef(null);

  const axiosPrivate = useAxiosPrivate();
  const { mutateGoals } = useNewGoal();

  useEffect(() => {
    if (isActive) {
      setName('');
      setDescription('');
      setNumberOfDays(21);
      setStartDate(getDateOnlyString(new Date()));
    }
  }, [isActive])

  useEffect(() => {
    if (isLoading) {
      submitButton.current.disabled = true;
    } else {
      submitButton.current.disabled = false;
    }
  }, [isLoading])

  const submitForm = async (e) => {
    setIsloading(true);
    e.preventDefault();
    // TODO: get templates from server
    const newData = { name: name, description: description, daysNumber: numberOfDays,
      dateStart: startDate, templateId: '1896477c-94ea-4b04-b523-75b382328e88'}
    try{
      const response = await axiosPrivate.post(goalsUrlEndpoint, JSON.stringify(newData));
      mutateGoals(response.data);
    }
    catch (error) {
      console.log(error);
      toast.error('Something went wrong');
    }
    finally{
      setIsloading(false);
      setIsActive(false);
    }
  }

  return (
    <ModalBase isActive={isActive} setIsActive={setIsActive}>
        <form onSubmit={submitForm}>
          <div className='base-modal-header'>
            <span>Create new goal</span>
          </div>

          <div className='base-modal-body'>
            <label htmlFor='goal-modal-name-input'>Name</label>
            <input id='goal-modal-name-input' type='text' autoComplete='off' value={name} onChange={e => setName(e.target.value)} required/>

            <label htmlFor='goal-modal-description-input'>Description (optional)</label>
            <input id='goal-modal-description-input' type='text' autoComplete='off' value={description} onChange={e => setDescription(e.target.value)}/>

            <label htmlFor='goal-modal-days-number-input'>Number of days</label>
            <input id='goal-modal-days-number-input' type='number' min='1' value={numberOfDays} onChange={e => setNumberOfDays(e.target.value)} required/>

            <label htmlFor='goal-modal-date-start-input'>Start date</label>
            <input id='goal-modal-date-start-input' type='date' min='1950-12-12' value={startDate} onChange={e => setStartDate(e.target.value)} required/>

            <label htmlFor='goal-modal-template-input'>Template</label>
            <input disabled={true} id='goal-modal-template-input' type='text' value="Habbit" required/>
          </div>

          <div className="base-modal-footer">
            <button type="submit" disabled={isLoading} ref={submitButton}>Save</button>
            <button type="button" onClick={() => setIsActive(false)}>Cancel</button>
          </div>
        </form>
    </ModalBase>
  )
}

export default GoalModal