import { useContext } from "react"
import CalendarContext from "../Context/CalendarProvide"

const useCalendar = () => {
  return useContext(CalendarContext);
}

export default useCalendar