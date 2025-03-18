import axios from 'axios';

const API_URL = 'https://localhost:7094/api/UserAuth'; 

const axiosInstance = axios.create();

axiosInstance.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  
  if (token) {
    config.headers['Authorization'] = `Bearer ${token}`;
  }

  return config;
}, (error) => {
  return Promise.reject(error);
});

export const register = async (userData) => {
  try {
    const response = await axios.post(`${API_URL}/Register`, userData);
    return response.data;
  } catch (error) {
    throw error.response.data;
  }
};

export const login = async (userData) => {
  try {
    const response = await axios.post(`${API_URL}/Login`, userData);
    return response.data;
  } catch (error) {
    throw error.response.data;
  }
};
