/**
 * Generates an array of the last month's days based on the given date and number.
 *
 * @param {Date} date - The date used to determine the month and year.
 * @param {number} number - The number of days to generate.
 * @return {Array} An array of the last month's dates.
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
 * Returns the number of days in the given month and year.
 *
 * @param {Date} date - the date for which to calculate the number of days
 * @return {number} the number of days in the given month
 */
export const getNumberOfDaysInMonth = (date) => {
    const month = date.getMonth();
    const year = date.getFullYear();
    return new Date(year, month + 1, 0).getDate();
}