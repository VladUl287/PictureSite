import { PictureModel } from './PictureModel';
import { IUser } from './IUser';
import { TagModel } from './TagModel';

export interface IState {
    auth: {
        user: IUser,
        isAuth: boolean
    }
    picture: {
        pictures: PictureModel[]
        tags: TagModel[]
    }
}