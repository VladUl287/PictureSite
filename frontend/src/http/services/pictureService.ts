import instance from "..";
import { TagModel } from "../../models/TagModel";

const getPictures = () => {
    return instance.get('api/picture/all');
}

const getPicturesByTag = (id: number) => {
    return instance.get('api/picture/tag/' + id);
}

const createImage = (file: File, tags: TagModel[]) => {
    const formData = new FormData();
    formData.append('picture', file, file.name);
    tags.forEach((tag, i) => {
        formData.append(`tags[${i}][id]`, tag.id.toString());
        formData.append(`tags[${i}][name]`, tag.name);
    });    
    return instance.post('api/picture/create', formData);
}

const downloadImage = (id: number, width: number, height: number) => {
    return instance.get(`api/picture/download/${id}?width=${width}&height=${height}`, {
        responseType: 'blob'
    });
}

const getMainPicture = (id: number) => {
    return instance.get(`api/picture/main/${id}`);
}

const searchTags = (name: string) => {
    return instance.get(`api/tag/search?name=${name}`);
}

export const pictureService = {
    getMainPicture,
    getPicturesByTag,
    getPictures,
    downloadImage,
    createImage,
    searchTags,
}