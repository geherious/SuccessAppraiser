
export const goalsUrlEndpoint = '/goals'

export const getGoals = async(axios) => {
    console.log('here');
    const response = await axios.get(goalsUrlEndpoint);
    return response.data;
}

export const addGoal = async(axios, name, description, daysNumber, dateStart, templateId) => {
    const response = await axios.post(goalsUrlEndpoint, {
        name, description, daysNumber, dateStart, templateId
    });
    return response.data;
}