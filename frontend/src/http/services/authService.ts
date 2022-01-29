import { IAuthResponse } from '../../models/responses/IAuthResponse';
import axios, { AxiosResponse } from 'axios';
import instance, { BASE_URL } from '..';

const login = (email: string, password: string): Promise<AxiosResponse<IAuthResponse>> => {
    return instance.post<IAuthResponse>('api/auth/login',
        {
            email,
            password
        });
}

const register = (email: string, login: string, password: string): Promise<AxiosResponse<IAuthResponse>> => {
    return instance.post('api/auth/register',
        {
            email,
            login,
            password
        });
}

const logout = (): Promise<void> => {
    return instance.post('api/auth/logout');
}

const refresh = (): Promise<AxiosResponse<IAuthResponse>> => {
    return axios.post<IAuthResponse>(BASE_URL + 'api/auth/refresh', {}, {
        withCredentials: true
    });
}

export const authService = {
    login,
    logout,
    register,
    refresh
}