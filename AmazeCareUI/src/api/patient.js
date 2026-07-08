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

export async function getAllPatients() {
  try {
    const response = await client.get('/api/Patient');
    return Array.isArray(response.data) ? response.data : (response.data.data || []);
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to fetch patients.'));
  }
}

export async function getPatientById(id) {
  try {
    const response = await client.get(`/PatientBy${id}`);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to fetch patient details.'));
  }
}

export async function addPatient(patientData) {
  try {
    const response = await client.post('/AddPatient', patientData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to add patient.'));
  }
}

export async function removePatient(id) {
  try {
    const response = await client.delete(`/api/Patient/RemovePatient${id}`);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to remove patient.'));
  }
}

export async function updatePatient(patientData) {
  try {
    const response = await client.put('/UpdatePatient', patientData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to update patient.'));
  }
}
