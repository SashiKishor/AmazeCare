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

export async function login(credentials) {
  try {
    const response = await client.post('/api/Auth/login', credentials);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Login failed.'));
  }
}

export async function registerPatientUser(userData) {
  try {
    const response = await client.post('/CreateNewUser', userData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Registration failed.'));
  }
}

export async function createDoctorOrAdminUser(adminUserData) {
  try {
    const response = await client.post('/AdminAccessCreate', adminUserData);
    return response.data;
  } catch (error) {
    throw new Error(getErrorMessage(error, 'Failed to create user.'));
  }
}
