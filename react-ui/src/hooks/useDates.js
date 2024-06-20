import useSWR from "swr";
import useHomeStore from "../Store/useHomeStore";
import { getGoalDateByMonth } from "../api/goalApi";
import useAxiosPrivate from "./useAxiosPrivate";
import { useCallback, useMemo } from "react";
import { compareDateOnly } from "../Services/Calendar/calendarService";

const useDates = () => {
  const axiosPrivate = useAxiosPrivate();
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

  const shouldFetch = activeGoal;
  const fetchSettings = { revalidateOnFocus: false, revalidateIfStale: false, shouldRetryOnError: false };

  const { data: lastMonthDates, mutate: mutateLastMonth, isLoading: lastMonthIsLoading } = useSWR(shouldFetch ? getKeyWithArgs(lastMonth) : null,
    (args) => axiosPrivate.get(getGoalDateByMonth, { params: getParams(args.year, args.month) }).then(res => res.data), fetchSettings);

  const { data: currentMonthDates, mutate: mutateCurrentMonth, isLoading: currentMonthIsLoading } = useSWR(shouldFetch ? getKeyWithArgs(currentDateArea) : null,
    (args) => axiosPrivate.get(getGoalDateByMonth, { params: getParams(args.year, args.month) }).then(res => res.data), fetchSettings);

  const { data: nextMonthDates, mutate: mutateNextMonth, isLoading: nextMonthIsLoading } = useSWR(shouldFetch ? getKeyWithArgs(nextMonth) : null,
    (args) => axiosPrivate.get(getGoalDateByMonth, { params: getParams(args.year, args.month) }).then(res => res.data), fetchSettings);

  const dates = useMemo(() => {
    if (lastMonthDates && currentMonthDates && nextMonthDates) {
      return [
        ...lastMonthDates,
        ...currentMonthDates,
        ...nextMonthDates
      ]
    }
    else {
      return null
    }
  }, [lastMonthDates, currentMonthDates, nextMonthDates]);

  function insertNewDate(array, newObject) {
    let index = array.findIndex(element => {
      return compareDateOnly(element.date, newObject.date) > 0;
    });
    if (index === -1) {
      array.push(newObject);
    } else {
      array.splice(index, 0, newObject);
    }
    return array;
  }

  const mutate = useCallback((date, newObject) => {
    const monthDiff = date.getMonth() - currentDateArea.getMonth();
    switch (monthDiff) {
      case 0:
        mutateCurrentMonth(insertNewDate(currentMonthDates, newObject));
        break;
      case -1:
        mutateLastMonth(insertNewDate(lastMonthDates, newObject));
        break;
      case 1:
        mutateNextMonth(insertNewDate(nextMonthDates, newObject));
        break;
      default:
        throw new Error('Invalid month difference in dates mutation');
    }
  }, [activeGoal, lastMonthDates, currentMonthDates, nextMonthDates]);

  return {
    dates,
    isLoading: lastMonthIsLoading || currentMonthIsLoading || nextMonthIsLoading,
    mutate
  }
};

export default useDates;