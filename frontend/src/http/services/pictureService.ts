import instance from "..";
import { Tag } from "../../models/Tag";

const getPictures = () => {
    return instance.get('api/picture/all');
}

const createImage = (file: File, tags: Tag[]) => {
    const formData = new FormData();
    formData.append('image', file, file.name);
    tags.forEach((tag, i) => {
        formData.append(`tags[${i}]['id']`, tag.id.toString());
        formData.append(`tags[${i}]['name']`, tag.name);
    });
    return instance.post('api/picture/create', formData);
}

const getTags = () => {
    return instance.get('api/tag/all');
}

const searchTags = (name: string) => {
    return instance.get('api/tag/search?name=' + name);
}

export const pictureService = {
    getPictures,
    createImage,
    getTags,
    searchTags
}