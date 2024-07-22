import useSWR from "swr";
import useAxiosPrivate from "./useAxiosPrivate";
import { goalsUrlEndpoint } from "../api/goalApi";
import useHomeStore from "../Store/useHomeStore";
import { useEffect, useMemo } from "react";

const useNewGoal = () => {
  const axiosPrivate = useAxiosPrivate();
  const activeGoal = useHomeStore(state => state.activeGoal);
  const setActiveGoal = useHomeStore(state => state.setActiveGoal);

  const {
    data: goals,
    isLoading: IsLoadingGoals
  } = useSWR(goalsUrlEndpoint, axiosPrivate.get, { revalidateOnFocus: false });

  useEffect(() => {
    if (activeGoal === null && goals) {
      setActiveGoal(goals.data[0])
    }
  }, [goals])

  return {
    goals: goals?.data,
    IsLoadingGoals
  }
};

export default useNewGoal;