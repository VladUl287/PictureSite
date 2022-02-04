import { Dispatch } from 'react';
import { authService } from '../../http/services/authService';
import { LOGIN, LOGOUT } from '../types';

export const userLogin = (email: string, password: string) => async (dispatch: Function) => {
    let result = await authService.login(email, password);

    if (result.status === 200) {
        localStorage.setItem('token', result.data.accessToken);
    
        dispatch({
            type: LOGIN,
            payload: {
                token: result.data.accessToken,
            }
        });
    }
}

export const userLogout = () => async (dispatch: Function) => {
    try {
        await authService.logout();
    } finally {
        dispatch({
            type: LOGOUT
        });
    }
}

export const checkAuth = () => async (dispatch: Dispatch<any>) => {
    try {
        const result = await authService.refresh();
        localStorage.setItem('token', result.data.accessToken);

        dispatch({
            type: LOGIN,
            payload: {
                token: result.data.accessToken,
            }
        });
    } catch {
        localStorage.removeItem('token');
    }
}