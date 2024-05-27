import useSWR from "swr";
import useHomeStore from "../Store/useHomeStore";
import { getGoalDateByMonth } from "../api/goalApi";
import useAxiosPrivate from "./useAxiosPrivate";
import { useCallback, useMemo } from "react";

const useDates = () => {
  const { isConfiguring, axiosPrivate } = useAxiosPrivate();
  const activeGoal = useHomeStore(state => state.activeGoal);

  const getKeyWithArgs = (date) => {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    return { url: getGoalDateByMonth + activeGoal?.id + year + month, year, month };
  }

  const getParams = (year, month) => {
    return { date: `${year}-${month}-01`, goalId: activeGoal.id };
  };

  const currentDateArea = useHomeStore(state => state.currentDateArea);
  const lastMonth = new Date(currentDateArea.getFullYear(), currentDateArea.getMonth() - 1, 1);
  const nextMonth = new Date(currentDateArea.getFullYear(), currentDateArea.getMonth() + 1, 1);

  const shouldFetch = !isConfiguring && activeGoal;
  const fetchSettings = { revalidateOnFocus: false, revalidateIfStale: false };

  const { data: lastMonthDates, mutate: mutateLastMonth } = useSWR(shouldFetch ? getKeyWithArgs(lastMonth) : null,
    (args) => axiosPrivate.get(getGoalDateByMonth, { params: getParams(args.year, args.month) }), fetchSettings);

  const { data: currentMonthDates, mutate: mutateCurrentMonth } = useSWR(shouldFetch ? getKeyWithArgs(currentDateArea) : null,
    (args) => axiosPrivate.get(getGoalDateByMonth, { params: getParams(args.year, args.month) }), fetchSettings);

  const { data: nextMonthDates, mutate: mutateNextMonth } = useSWR(shouldFetch ? getKeyWithArgs(nextMonth) : null,
    (args) => axiosPrivate.get(getGoalDateByMonth, { params: getParams(args.year, args.month) }), fetchSettings);

  const dates = useMemo(() => {
    if (lastMonthDates && currentMonthDates && nextMonthDates) {
      return [
        ...lastMonthDates.data,
        ...currentMonthDates.data,
        ...nextMonthDates.data
      ]
    }
    else {
      return null
    }
  }, [lastMonthDates, currentMonthDates, nextMonthDates]);

  //     const mutate = useCallback((date, newObject) => {
  //         const monthDiff = date.getMonth() - currentDateArea.getMonth();
  //         switch (monthDiff) {
  //             case 0:
  //                 mutateCurrentMonth(newObject);
  //                 break;
  //             case -1:
  //                 mutateLastMonth(newObject);
  //                 break;
  //             case 1:
  //                 mutateNextMonth(newObject);
  //                 break;
  //             default:
  //                 throw new Error('Invalid month difference in dates mutation');
  //         }
  // }, [activeGoal, lastMonthDates, currentMonthDates, nextMonthDates]);

  return {
    dates
  }
};

export default useDates;