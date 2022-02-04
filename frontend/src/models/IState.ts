import { PictureModel } from './PictureModel';
import { IUser } from './IUser';

export interface IState {
    auth: {
        user: IUser,
        isAuth: boolean
    }
    picture: {
        pictures: PictureModel[]
    }
}