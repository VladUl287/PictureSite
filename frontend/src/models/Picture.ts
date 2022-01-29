import { Tag } from './Tag';

export class Picture {
    constructor(
        public view: string,
        public tags: Tag[],
        public id?: number
    ) { }
}