import styles from './Picture.module.css';
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import { pictureService } from "../../http/services/pictureService";
import { PictureModel } from "../../models/PictureModel";
import Tags from '../tag/Tags';
import { TagModel } from '../../models/TagModel';

type Size = {
    width: number,
    height: number
}

const Picture = () => {

    const { id } = useParams();
    const navigate = useNavigate();
    const [picture, setPicture] = useState<PictureModel>();
    const [sizes, setSizes] = useState<Size[]>([]);
    
    if (!id) {
        navigate("/Main");
    }

    const pictureId = +id!;

    useEffect(() => {
        pictureService.getMainPicture(pictureId).then(
            (data: any) => {
                setPicture(data.data);

                let sizes = new Array<Size>();
                sizes.push({ width: data.data.originalWidth, height: data.data.originalHeight } as Size);

                let height = Math.round(data.data.originalHeight / (data.data.originalWidth / 1920));
                sizes.push({ width: 1920, height } as Size);

                height = Math.round(data.data.originalHeight / (data.data.originalWidth / 1280));
                sizes.push({ width: 1280, height } as Size);

                height = Math.round(data.data.originalHeight / (data.data.originalWidth / 640));
                sizes.push({ width: 640, height } as Size);

                setSizes(sizes);
            });
    }, []);

    const download = (size: Size) => {
        pictureService.downloadImage(pictureId, size.width, size.height)
            .then(data => {
                const url = window.URL.createObjectURL(data.data);
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', 'image.jpeg');
                document.body.appendChild(link);
                link.click();
            });
    }

    const clickHandle = (tag: TagModel) => {
        navigate('/main?tag=' + tag.id);
    }

    return (
        <div>
            {picture ? (
                <div className={styles.pictureWrap}>
                    <div>
                        <img src={picture.view} alt="" />
                    </div>
                    <div>
                        <div className={styles.downloadZone}>
                            <label
                                htmlFor="toogleItems"
                                className={styles.downoladBtn}
                            >
                                Скачать изображение
                            </label>
                            <input
                                hidden
                                type="checkbox"
                                id="toogleItems"
                                className={styles.downoladInput}
                            />
                            <div className={styles.downloadItems}>
                                {sizes.map((size, i) => (
                                    <div key={i}>
                                        <button onClick={() => download(size)}>{size.width}x{size.height}</button>
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                    <Tags tags={picture.tags} clickHandle={clickHandle} />
                </div>
            ) :
                <div>Loading</div>}
        </div>
    );
}

export default Picture;