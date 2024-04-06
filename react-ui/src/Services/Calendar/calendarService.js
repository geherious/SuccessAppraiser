/**
 * Generates an array of the last month's days based on the given date and number.
 *
 * @param {Date} date - The date used to determine the month and year.
 * @param {number} number - The number of days to generate.
 * @return {Array<Date>} An array of the last month's dates.
 */
export const getLastMonthDates = (date, number) => {
    const month = date.getMonth();
    const year = date.getFullYear();
    const lastDay = new Date(year, month, 0);
    let days = [];
    for (let i = number - 1; i >= 0; i--) {
        days.push(new Date(lastDay.getTime() - i * 24 * 60 * 60 * 1000));
    }
    return days;
}

/**
 * Returns an array of dates for the given month and year.
 *
 * @param {Date} date - The date object representing the month and year.
 * @return {Array<Date>} An array of dates for the given month and year.
 */
export const getDatesInMonth = (date) => {
    const month = date.getMonth();
    const year = date.getFullYear();
    const lastDay = new Date(year, month + 1, 0);
    let days = [];
    for (let i = 1; i <= lastDay.getDate(); i++) {
        days.push(new Date(year, month, i));
    }
    return days
}

/**
 * Generate an array of dates for the next 'number' days after the input 'date'.
 *
 * @param {Date} date - the starting date
 * @param {number} number - the number of days to generate dates for
 * @return {Array<Date>} an array of dates for the next 'number' days
 */
export const getNextMonthDates = (date, number) => {
    const month = date.getMonth();
    const year = date.getFullYear();
    const lastDay = new Date(year, month + 1, 0);
    let days = [];
    for (let i = 1; i <= number; i++) {
        days.push(new Date(lastDay.getTime() + i * 24 * 60 * 60 * 1000));
    }
    return days;
}

/**
 * Returns the last day of the month for the given date.
 *
 * @param {Date} date - the input date
 * @return {Date} the last day of the month
 */
export const getLastDayInMonth = (date) => {
    const month = date.getMonth();
    const year = date.getFullYear();
    const lastDay = new Date(year, month + 1, 0);
    return lastDay
}

/**
 * Calculates the day of the week for a given date, taking into account a shift.
 *
 * @param {Date} date - The date for which to calculate the weekday.
 * @param {number} shift - The number of days to shift the weekday calculation. E.g for monday to be 0 shift is 1
 * @return {number} The calculated weekday.
 */
export const getWeekDay = (date, shift = 0) => {
    const day = date.getDay();
    return (day + 7 - shift) % 7
}