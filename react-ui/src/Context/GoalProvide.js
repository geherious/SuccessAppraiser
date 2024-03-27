import { createContext, useState } from "react";
import useSWR from "swr";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { goalsUrlEndpoint } from "../api/GoalApi";
import useAuth from "../hooks/useAuth";

const GoalContext = createContext({});

export const GoalProvider = ({children}) => {
    const { isConfiguring, axiosPrivate } = useAxiosPrivate();
    const { auth } = useAuth();
    const {
        data: goals,
        error: goalError,
        mutate: goalMutate,
        isLoading: goalIsLoading
      } = useSWR(auth.accessToken && !isConfiguring ? goalsUrlEndpoint : null, axiosPrivate.get);

    return (
        <GoalContext.Provider value={{goals, goalError, goalMutate, goalIsLoading}}>
            {children}
        </GoalContext.Provider>
    );
}

export default GoalContext;