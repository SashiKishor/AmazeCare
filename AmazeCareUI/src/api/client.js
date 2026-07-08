import axios from 'axios';

const client = axios.create({
  baseURL: 'https://localhost:7056',
  headers: {
    'Content-Type': 'application/json'
  }
});

client.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

client.interceptors.response.use(
  (response) => {
    if (response.data && typeof response.data === 'object' && !Array.isArray(response.data)) {
      const normalized = {};
      for (const key of Object.keys(response.data)) {
        normalized[key.toLowerCase()] = response.data[key];
        normalized[key] = response.data[key];
      }

      const status = normalized.statuscode || response.status;
      normalized.success = status >= 200 && status < 300;

      if (normalized.data !== undefined) {
        normalized.Data = normalized.data;
      } else if (normalized.Data !== undefined) {
        normalized.data = normalized.Data;
      }

      response.data = normalized;
    }
    return response;
  },
  (error) => {
    if (error.response && error.response.data && typeof error.response.data === 'object' && !Array.isArray(error.response.data)) {
      const normalized = {};
      for (const key of Object.keys(error.response.data)) {
        normalized[key.toLowerCase()] = error.response.data[key];
        normalized[key] = error.response.data[key];
      }
      normalized.success = false;

      if (normalized.data !== undefined) {
        normalized.Data = normalized.data;
      } else if (normalized.Data !== undefined) {
        normalized.data = normalized.Data;
      }

      error.response.data = normalized;
    }
    return Promise.reject(error);
  }
);

export default client;
