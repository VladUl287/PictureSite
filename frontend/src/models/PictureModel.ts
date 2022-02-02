import { TagModel } from './TagModel';

export class PictureModel {
    constructor(
        public id: number,
        public view: string,
        public originalWidth: number,
        public originalHeight: number,
        public tags: TagModel[],
    ) { }
}