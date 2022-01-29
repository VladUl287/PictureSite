import axios from 'axios';
import { authService } from './services/authService';

export const BASE_URL = 'https://localhost:44346/';

const instance = axios.create({
    baseURL: BASE_URL,
    withCredentials: true
});

instance.interceptors.request.use((config) => {
    config.headers = {
        Authorization: 'Bearer ' + localStorage.getItem('token')
    };
    return config;
});

instance.interceptors.response.use((config) => {
    return config;
}, async (error) => {
    const original = error.config;
    if(error.response.status === 401 && error.config && !error.config._isRetry) {
        try {
            const result = await authService.refresh();
            localStorage.setItem('token', result.data.token);
            return instance.request(original);
        } catch {}
    } else {
        return Promise.reject(error);
    }
});

export default instance;