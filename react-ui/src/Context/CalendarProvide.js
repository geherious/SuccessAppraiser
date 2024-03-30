import { createContext, useState } from "react";

const CalendarContext = createContext({});

export const CalendarProvider = ({children}) => {

    const [currentDateArea, setCurrentDateArea] = useState(new Date(new Date().getFullYear(), new Date().getMonth(), 1));

    return (
        <CalendarContext.Provider value={{currentDateArea, setCurrentDateArea}}>
            {children}
        </CalendarContext.Provider>
    );
}

export default CalendarContext;