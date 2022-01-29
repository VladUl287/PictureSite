import { pictureReducer } from './reducers/pictureReducer';
import { combineReducers } from 'redux';
import { authReducer } from './reducers/authReducer';

export const rootReducer = combineReducers({
    auth: authReducer,
    picture: pictureReducer
});