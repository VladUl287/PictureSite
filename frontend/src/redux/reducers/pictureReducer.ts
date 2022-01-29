import { Picture } from "../../models/Picture";
import { Tag } from "../../models/Tag";
import { SET_PICTURES, SET_TAGS } from "../types";

const initialState = {
    pictures: new Array<Picture>(),
    tags: new Array<Tag>()
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
        pictures: Picture[],
        tags: Tag[]
    }
}