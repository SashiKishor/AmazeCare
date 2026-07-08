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

export async function addPrescription(prescriptionData) {
  try {
    const response = await client.post('/AddPrescription', prescriptionData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to add prescription.'));
  }
}

export async function getPrescriptionById(prescriptionId) {
  try {
    const response = await client.get(`/GetPrescriptionBy${prescriptionId}`);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to fetch prescription details.'));
  }
}

export async function getAllPrescriptionsForPatient(recordId) {
  try {
    const response = await client.get(`/AllPrescrpitionForPatient${recordId}`);
    return response.data.data || [];
  } catch (error) {
    if (error.response && error.response.status === 404) return [];
    throw new Error(getErrorMessage(error, 'Failed to fetch prescriptions.'));
  }
}

export async function updatePrescription(prescriptionData) {
  try {
    const response = await client.put('/UpdatePrescription', prescriptionData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to update prescription.'));
  }
}
