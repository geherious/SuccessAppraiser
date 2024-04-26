import { createContext, useEffect, useState } from "react";
import useSWR from "swr";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { goalsUrlEndpoint } from "../api/GoalApi";
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

    useEffect(() => {
        if (activeGoal == null && goals) {
            setActiveGoal(goals.data[0].id);
        }
    }, [goals]);

    return (
        <GoalContext.Provider value={{goals: goals?.data, goalError, goalMutate, goalIsLoading, activeGoal, setActiveGoal}}>
            {children}
        </GoalContext.Provider>
    );
}

export default GoalContext;