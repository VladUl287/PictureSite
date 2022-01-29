import jwtDecode from 'jwt-decode';
import { IToken } from '../../models/IToken';
import { IUser } from '../../models/IUser';
import { LOGIN, LOGOUT } from '../types';

const initialState = {
    user: {} as IUser,
    isAuth: false,
}

export const authReducer = (state = initialState, action: { type: string, payload: { token: string } }) => {
    const { type, payload } = action;
    switch (type) {
        case LOGIN:
            const user = jwtDecode<IToken>(payload.token);
            return {
                ...state,
                user: {
                    id: user.id,
                    email: user.email,
                    role: user.role
                },
                isAuth: true
            }
        case LOGOUT:
            localStorage.removeItem('token');
            return {
                ...state,
                user: {} as IUser,
                isAuth: false
            }
        default:
            return state;
    }
}