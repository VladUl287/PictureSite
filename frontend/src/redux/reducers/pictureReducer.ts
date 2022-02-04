import { PictureModel } from "../../models/PictureModel";
import { SET_PICTURES } from "../types";

const initialState = {
    pictures: new Array<PictureModel>()
}

export const pictureReducer = (state = initialState, action: PictureAction) => {
    const { type, payload } = action;
    switch (type) {
        case SET_PICTURES:
            return {
                ...state,
                pictures: payload.pictures
            }
        default:
            return state;
    }
}

type PictureAction = {
    type: string,
    payload: {
        pictures: PictureModel[]
    }
}