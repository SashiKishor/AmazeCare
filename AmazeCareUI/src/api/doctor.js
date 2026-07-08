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

export async function getAllDoctors() {
  try {
    const response = await client.get('/GetAllDoctors');
    return response.data.data || [];
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to fetch doctors.'));
  }
}

export async function getDoctorById(id) {
  try {
    const response = await client.get(`/api/Doctor/DoctorBy${id}`);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to fetch doctor details.'));
  }
}

export async function addDoctor(doctorData) {
  try {
    const response = await client.post('/AddDoctor', doctorData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to add doctor.'));
  }
}

export async function removeDoctor(id) {
  try {
    const response = await client.delete(`/RemoveDoctorBy${id}`);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to delete doctor.'));
  }
}

export async function updateDoctor(doctorData) {
  try {
    const response = await client.put('/api/Doctor/UpdateDoctor', doctorData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to update doctor.'));
  }
}

export async function getAvailableDoctors(filters) {
  try {
    const response = await client.get('/availableDoctors', { params: filters });
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to fetch available doctors.'));
  }
}
