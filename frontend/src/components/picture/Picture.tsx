import styles from './Picture.module.css';
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import { pictureService } from "../../http/services/pictureService";
import { PictureModel } from "../../models/PictureModel";

type Size = {
    width: number,
    height: number
}

const Picture = () => {

    const { id } = useParams();
    const navigate = useNavigate();

    if (!id) {
        navigate("/Main");
    }

    const [picture, setPicture] = useState<PictureModel>();
    const [sizes, setSizes] = useState<Size[]>([]);

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
                link.setAttribute('download', 'file.jpeg');
                document.body.appendChild(link);
                link.click();
            });
    }

    const clickHandle = (id: number) => {
        navigate('/main?tag=' + id);
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
                    <div className={styles.tagZone}>
                        {picture.tags.map(tag => (
                            <div
                                key={tag.id}
                                className={styles.selectedTag}
                                onClick={() => clickHandle(tag.id)}
                            >
                                <p>{tag.name}</p>
                            </div>
                        ))}
                    </div>
                </div>
            ) :
                <div>none</div>}
        </div>
    );
}

export default Picture;