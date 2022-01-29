import React, { BaseSyntheticEvent } from 'react';
import { Tag } from "../../models/Tag";
import styles from './Create.module.css';
import { useEffect, useState } from "react";
import { IState } from "../../models/IState";
import { useDispatch, useSelector } from "react-redux";
import { getTags } from "../../redux/actions/pictureActions";
import { pictureService } from "../../http/services/pictureService";

const CreateImage = () => {
    const dispatch = useDispatch();
    // const stateTags = useSelector((state: IState) => state.picture.tags);
    const hiddenFileInput = React.useRef<HTMLInputElement>(null);
    const selectImg = React.useRef<HTMLImageElement>(null);
    const [picture, setPicture] = useState<File | null>();
    const [tags, setTags] = useState<Tag[]>([]);
    const [selectTags, setSelectTags] = useState<Tag[]>([]);

    const createImage = () => {
        // let tags: Tag[] = data.tags.map(el => new Tag(el, stateTags.find((e: Tag) => e.id == el)!.name));
        pictureService.createImage(picture!, []);
    }


    const handleChange = (event: BaseSyntheticEvent) => {
        setPicture(event.target.files[0]);
        let fr = new FileReader();
        fr.onload = () => {
            if (selectImg.current && typeof fr.result === 'string') {
                selectImg.current.src = fr.result;
            }
        }
        fr.readAsDataURL(event.target.files[0]);
    };

    const handleClick = () => {
        if (hiddenFileInput.current) {
            hiddenFileInput.current.click();
        }
    };

    const tagChange = (e: BaseSyntheticEvent) => {
        if (e.target.value.length == 0) {
            setTags([]);
            return;
        }
        pictureService.searchTags(e.target.value).then(
            (data: any) => {
                setTags(data.data);
            }
        );
    }

    const select = (tag: Tag) => {
        let index = selectTags.findIndex(e => e.id == tag.id);
        if (index > -1) {
            return;
        }
        else {
            setSelectTags([...selectTags, tag]);
        }
    }


    const unselect = (tag: Tag) => {
        let index = selectTags.findIndex(e => e.id == tag.id);
        if (index > -1) {
            selectTags.splice(index, 1);
            setSelectTags([...selectTags]);
        }
    }

    useEffect(() => {
        dispatch(getTags());
    }, []);

    return (
        <div className={styles.createWrap}>
            <div className={styles.selectFileWrap}>
                <input
                    type='file'
                    ref={hiddenFileInput}
                    onChange={handleChange}
                    accept="image/png, image/jpg, image/jpeg"
                    hidden
                />
                <button
                    className={styles.fileInput}
                    onClick={handleClick}>
                    Выбрать изображение
                </button>
                <div>
                    {picture ? <p>{picture.name}</p> : <></>}
                </div>
            </div>
            {/* <div className={styles.tagZone}>
                    <div className={styles.searchTags}>
                        <input type="text" onChange={tagChange} placeholder='теги' />
                        <div className={styles.searchResult}>
                            {tags.map((tag: Tag) => (
                                <div key={tag.id} onClick={() => select(tag)}>
                                    <p>{tag.name}</p>
                                </div>
                            ))}
                        </div>
                    </div>
                    <div className={styles.selectedTags}>
                        {selectTags.map((tag: Tag) => (
                            <div key={tag.id} className={styles.selectedTag}>
                                <p>{tag.name}</p>
                                <span onClick={() => unselect(tag)}>X</span>
                            </div>
                        ))}
                    </div>
                </div>
                <div>
                    <img
                        src=""
                        alt="fff"
                        ref={selectImg}
                        className={styles.selectImg}
                    />
                </div>
                <div>
                    <button
                        className={styles.submitButton}
                        onClick={createImage}
                    >
                        Добавить изображение
                    </button>
                </div> */}
        </div>
    );
}

export default CreateImage; 