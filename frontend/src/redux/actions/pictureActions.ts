import { SET_PICTURES, SET_TAGS } from '../types';
import { pictureService } from '../../http/services/pictureService';

export const getBooks = () => async (dispatch: Function) => {
    let result = await pictureService.getPictures();
    console.log(result);
    dispatch({
        type: SET_PICTURES,
        payload: {
            pictures: result.data,
        }
    });
}

export const getTags = () => async (dispatch: Function) => {
    let result = await pictureService.getTags();
    
    dispatch({
        type: SET_TAGS,
        payload: {
            tags: result.data,
        }
    });
}