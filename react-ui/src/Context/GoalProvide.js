import { createContext, useEffect, useState } from "react";
import useSWR from "swr";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { goalsUrlEndpoint, getGoalDateByMonth } from "../api/GoalApi";
import useAuth from "../hooks/useAuth";
import useCalendar from "../hooks/useCalendar";

const GoalContext = createContext({});

export const GoalProvider = ({children}) => {
    const { isConfiguring, axiosPrivate } = useAxiosPrivate();
    const { auth } = useAuth();
    const {
        data: goals,
        error: goalError,
        mutate: goalMutate,
        isLoading: goalIsLoading
    } = useSWR(auth.accessToken && !isConfiguring ? goalsUrlEndpoint : null, axiosPrivate.get, {revalidateOnFocus: false});
    const [activeGoal, setActiveGoal] = useState(null);

    const {currentDateArea} = useCalendar();
    const year = currentDateArea.getFullYear();
    const month = String(currentDateArea.getMonth() + 1).padStart(2, '0');

    const key = getGoalDateByMonth + activeGoal?.id + year + month;

    const {
        data: dates
    } = useSWR(activeGoal ? key : null, () => axiosPrivate.get(getGoalDateByMonth, {params:{
        date: `${year}-${month}-01`,
        goalId: activeGoal.id
    }}), {revalidateOnFocus: false, revalidateIfStale: false});



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
            dates: dates?.data
            }}>
            {children}
        </GoalContext.Provider>
    );
}

export default GoalContext;