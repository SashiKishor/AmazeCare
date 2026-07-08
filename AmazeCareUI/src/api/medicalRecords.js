import client from './client';

function getErrorMessage(error, fallbackMessage) {
  if (error.response?.data?.message) {
    return error.response.data.message;
  }
  if (error.response?.data?.title) {
    return error.response.data.title;
  }
  if (error.message) {
    return error.message;
  }
  return fallbackMessage;
}

export async function addMedicalRecord(recordData) {
  try {
    const response = await client.post('/AddMedicalRecord', recordData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to add medical record.'));
  }
}

export async function getReportForRecord(recordId) {
  try {
    const response = await client.get(`/GetReportfor${recordId}`);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to fetch medical record details.'));
  }
}

export async function updateMedicalRecord(recordData) {
  try {
    const response = await client.put('/UpdateMedicalRecord', recordData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to update medical record.'));
  }
}
