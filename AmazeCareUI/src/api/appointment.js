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

export async function createNewAppointment(appointmentData) {
  try {
    const response = await client.post('/CreateNewAppointment', appointmentData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to create appointment.'));
  }
}

export async function getAllAppointments() {
  try {
    const response = await client.get('/GetAllAppointments');
    return response.data.data || [];
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to fetch appointments.'));
  }
}

export async function getAppointmentById(id) {
  try {
    const response = await client.get(`/AppointmentById${id}`);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to fetch appointment.'));
  }
}

export async function getUpcomingAppointmentsOfDoctor(doctorId) {
  try {
    const response = await client.get(`/UpcomingAppointmentsOfDoctors${doctorId}`);
    return response.data.data || [];
  } catch (error) {
    if (error.response && error.response.status === 404) return [];
    throw new Error(getErrorMessage(error, 'Failed to fetch upcoming appointments of doctor.'));
  }
}

export async function getAllAppointmentsOfTheDoctor(doctorId) {
  try {
    const response = await client.get(`/AllAppointmentsOfTheDoctor${doctorId}`);
    return response.data.data || [];
  } catch (error) {
    if (error.response && error.response.status === 404) return [];
    throw new Error(getErrorMessage(error, 'Failed to fetch all appointments of doctor.'));
  }
}

export async function getUpcomingAppointmentsOfPatient(patientId) {
  try {
    const response = await client.get(`/UpcomingAppointmentsOfPatient${patientId}`);
    return response.data.data || [];
  } catch (error) {
    if (error.response && error.response.status === 404) return [];
    throw new Error(getErrorMessage(error, 'Failed to fetch upcoming appointments of patient.'));
  }
}

export async function getAllAppointmentsOfThePatient(patientId) {
  try {
    const response = await client.get(`/AllAppointmentsOfThePatient${patientId}`);
    return response.data.data || [];
  } catch (error) {
    if (error.response && error.response.status === 404) return [];
    throw new Error(getErrorMessage(error, 'Failed to fetch all appointments of patient.'));
  }
}

export async function getUpcomingAppointments() {
  try {
    const response = await client.get('/UpcomingAppointments');
    return response.data.data || [];
  } catch (error) {
    if (error.response && error.response.status === 404) return [];
    throw new Error(getErrorMessage(error, 'Failed to fetch upcoming appointments.'));
  }
}

export async function getRequestedAppointments() {
  try {
    const response = await client.get('/RequestedAppointments');
    return response.data.data || [];
  } catch (error) {
    if (error.response && error.response.status === 404) return [];
    throw new Error(getErrorMessage(error, 'Failed to fetch requested appointments.'));
  }
}

export async function updateAppointmentStatus(statusData) {
  try {
    const response = await client.put('/UpdateStatus', statusData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to update appointment status.'));
  }
}

export async function rescheduleAppointment(rescheduleData) {
  try {
    const response = await client.put('/ResheduleAppointment', rescheduleData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to reschedule appointment.'));
  }
}

export async function deleteAppointment(id) {
  try {
    const response = await client.delete(`/DeleteAppointment${id}`);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to delete appointment.'));
  }
}
