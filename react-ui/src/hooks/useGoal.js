import { useContext } from "react"
import GoalContext from "../Context/GoalProvide"

const useGoal = () => {
  return useContext(GoalContext);
}

export default useGoal