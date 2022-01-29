import { Picture } from './Picture';
import { IUser } from './IUser';
import { Tag } from './Tag';

export interface IState {
    auth: {
        user: IUser,
        isAuth: boolean
    }
    picture: {
        pictures: Picture[]
        tags: Tag[]
    }
}