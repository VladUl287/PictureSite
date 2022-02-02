import { SET_PICTURES } from '../types';
import { pictureService } from '../../http/services/pictureService';

export const getPictures = () => async (dispatch: Function) => {
    let result = await pictureService.getPictures();
    
    dispatch({
        type: SET_PICTURES,
        payload: {
            pictures: result.data,
        }
    });
}

export const getPicturesByTag = (id: number) => async (dispatch: Function) => {
    let result = await pictureService.getPicturesByTag(id);
    
    dispatch({
        type: SET_PICTURES,
        payload: {
            pictures: result.data,
        }
    });
}
