import { PictureModel } from "../../models/PictureModel";
import { TagModel } from "../../models/TagModel";
import { SET_PICTURES, SET_TAGS } from "../types";

const initialState = {
    pictures: new Array<PictureModel>(),
    tags: new Array<TagModel>()
}

export const pictureReducer = (state = initialState, action: PictureAction) => {
    const { type, payload } = action;
    switch (type) {
        case SET_PICTURES:
            return {
                ...state,
                pictures: payload.pictures
            }
        case SET_TAGS:
            return {
                ...state,
                tags: payload.tags
            }
        default:
            return state;
    }
}

type PictureAction = {
    type: string,
    payload: {
        pictures: PictureModel[],
        tags: TagModel[]
    }
}