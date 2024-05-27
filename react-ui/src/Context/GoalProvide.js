import { createContext, useEffect, useState } from "react";
import useSWR from "swr";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { goalsUrlEndpoint, getGoalDateByMonth } from "../api/goalApi";
import useCalendar from "../hooks/useCalendar";

const GoalContext = createContext({});

export const GoalProvider = ({ children }) => {
  const { isConfiguring, axiosPrivate } = useAxiosPrivate();
  const {
    data: goals,
    error: goalError,
    mutate: goalMutate,
    isLoading: goalIsLoading
  } = useSWR(!isConfiguring ? goalsUrlEndpoint : null, axiosPrivate.get, { revalidateOnFocus: false });
  const [activeGoal, setActiveGoal] = useState(null);

  const { currentDateArea } = useCalendar();
  const lastMonth = new Date(currentDateArea.getFullYear(), currentDateArea.getMonth() - 1, 1);
  const nextMonth = new Date(currentDateArea.getFullYear(), currentDateArea.getMonth() + 1, 1);

  const getKeyWithArgs = (date) => {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    return { url: getGoalDateByMonth + activeGoal?.id + year + month, year, month };
  }

  const { data: lastMonthDates } = useSWR(!isConfiguring && activeGoal ? getKeyWithArgs(lastMonth) : null,
    (args) => axiosPrivate.get(getGoalDateByMonth, {
      params: {
        date: `${args.year}-${args.month}-01`,
        goalId: activeGoal.id
      }
    }), { revalidateOnFocus: false, revalidateIfStale: false });

  const { data: currentMonthDates } = useSWR(!isConfiguring && activeGoal ? getKeyWithArgs(currentDateArea) : null,
    (args) => axiosPrivate.get(getGoalDateByMonth, {
      params: {
        date: `${args.year}-${args.month}-01`,
        goalId: activeGoal.id
      }
    }), { revalidateOnFocus: false, revalidateIfStale: false });

  const { data: nextMonthDates } = useSWR(!isConfiguring && activeGoal ? getKeyWithArgs(nextMonth) : null,
    (args) => axiosPrivate.get(getGoalDateByMonth, {
      params: {
        date: `${args.year}-${args.month}-01`,
        goalId: activeGoal.id
      }
    }), { revalidateOnFocus: false, revalidateIfStale: false });

  const [dates, setDates] = useState(null);
  useEffect(() => {
    if (lastMonthDates && currentMonthDates && nextMonthDates) {
      setDates([...lastMonthDates.data, ...currentMonthDates.data, ...nextMonthDates.data]);
    }
  }, [lastMonthDates, currentMonthDates, nextMonthDates]);

  useEffect(() => {
    if (activeGoal == null && goals && !goalError) {
      setActiveGoal(goals.data[0]);
    }
  }, [goals]);

  return (
    <GoalContext.Provider value={{
      goals: goals?.data,
      goalError,
      goalMutate,
      goalIsLoading,
      activeGoal,
      setActiveGoal,
      dates
    }}>
      {children}
    </GoalContext.Provider>
  );
}

export default GoalContext;