import { useEffect, useRef, useState } from 'react';
import useHomeStore from '../../Store/useHomeStore';
import ModalBase from '../ModalBase/ModalBase';
import './GoalModal.css';
import 'react-toastify/dist/ReactToastify.css';
import useAxiosPrivate from '../../hooks/useAxiosPrivate';
import { getDateOnlyString } from '../../Services/Calendar/calendarService';
import { goalsUrlEndpoint, templatesUrlEndpoint } from '../../api/goalApi';
import { toast } from 'react-toastify';
import useNewGoal from '../../hooks/useNewGoal';
import useSWR from 'swr';
import LoaderDots from '../Loaders/LoaderDots';


const GoalModal = () => {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [numberOfDays, setNumberOfDays] = useState(21);
  const [startDate, setStartDate] = useState(getDateOnlyString(new Date()));
  const [templateId, setTemplateId] = useState('');

  const isActive = useHomeStore(state => state.goalModalIsActive);
  const setIsActive = useHomeStore(state => state.setGoalModalIsActive);

  const [isLoading, setIsloading] = useState(false);
  const submitButton = useRef(null);

  const axiosPrivate = useAxiosPrivate();
  const { mutateGoals } = useNewGoal();

  const fetchSettings = { revalidateOnFocus: false, revalidateIfStale: false, shouldRetryOnError: false };
  const {
    data: templates
  } = useSWR(templatesUrlEndpoint, axiosPrivate.get, fetchSettings);

  useEffect(() => {
    if (isActive) {
      setName('');
      setDescription('');
      setNumberOfDays(21);
      setStartDate(getDateOnlyString(new Date()));
    }
  }, [isActive])

  useEffect(() => {
    if (templates) {
      setTemplateId(templates.data[0].id);
    }
  }, [templates])

  const submitForm = async (e) => {
    setIsloading(true);
    e.preventDefault();
    
    const newData = {
      name: name, description: description, daysNumber: numberOfDays,
      dateStart: startDate, templateId: templateId
    }
    try {
      const response = await axiosPrivate.post(goalsUrlEndpoint, JSON.stringify(newData));
      mutateGoals(response.data);
    }
    catch (error) {
      console.log(error);
      toast.error('Something went wrong');
    }
    finally {
      setIsloading(false);
      setIsActive(false);
    }
  }

  return (
    <>
      {templates ? (
        <ModalBase isActive={isActive} setIsActive={setIsActive}>
          <form onSubmit={submitForm}>
            <div className='base-modal-header'>
              <span>Create new goal</span>
            </div>

            <div className='base-modal-body'>
              <label htmlFor='goal-modal-name-input'>Name</label>
              <input id='goal-modal-name-input' type='text' autoComplete='off' value={name} onChange={e => setName(e.target.value)} required />

              <label htmlFor='goal-modal-description-input'>Description (optional)</label>
              <input id='goal-modal-description-input' type='text' autoComplete='off' value={description} onChange={e => setDescription(e.target.value)} />

              <label htmlFor='goal-modal-days-number-input'>Number of days</label>
              <input id='goal-modal-days-number-input' type='number' min='1' value={numberOfDays} onChange={e => setNumberOfDays(e.target.value)} required />

              <label htmlFor='goal-modal-date-start-input'>Start date</label>
              <input id='goal-modal-date-start-input' type='date' min='1950-12-12' value={startDate} onChange={e => setStartDate(e.target.value)} required />

              <label htmlFor='goal-modal-template-input'>Template</label>
              <select id='goal-modal-template-input' value={templateId} onChange={e => setTemplateId(e.target.value)}>
                {templates.data.map(template => (
                  <option key={template.id} value={template.id}>{template.name}</option>
                ))}
              </select>
            </div>

            <div className="base-modal-footer">
              <button type="submit" disabled={isLoading} ref={submitButton}>Save</button>
              <button type="button" onClick={() => setIsActive(false)}>Cancel</button>
            </div>
          </form>
        </ModalBase>) :
        (
          <ModalBase isActive={isActive} setIsActive={setIsActive}>
            <LoaderDots />
          </ModalBase>
        )
      }
    </>
  )
}

export default GoalModal