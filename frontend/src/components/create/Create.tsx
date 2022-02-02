import { SyntheticEvent, useState } from "react";
import { TagModel } from "../../models/TagModel";
import styles from './Create.module.css';
import React, { BaseSyntheticEvent } from 'react';
import { pictureService } from "../../http/services/pictureService";

const CreateImage = () => {
    const [tags, setTags] = useState<TagModel[]>([]);
    const [picture, setPicture] = useState<File | null>();
    const [selectTags, setSelectTags] = useState<TagModel[]>([]);

    const selectImg = React.useRef<HTMLImageElement>(null);
    const hiddenFileInput = React.useRef<HTMLInputElement>(null);

    const createImage = async () => {
        if (picture && selectTags.length > 0) {
            await pictureService.createImage(picture, selectTags);
            setSelectTags([]);
            setPicture(null);
            setTags([]);
            if (selectImg.current) {
                selectImg.current.src = '';
            }
        }
    }

    const handleChange = (event: BaseSyntheticEvent) => {
        setPicture(event.target.files[0]);
        if (event.target.files[0]) {
            let fr = new FileReader();
            fr.onload = () => {
                if (selectImg.current && typeof fr.result === 'string') {
                    selectImg.current.src = fr.result;
                }
            }
            fr.readAsDataURL(event.target.files[0]);
        }
    };

    const handleClick = () => {
        if (hiddenFileInput.current) {
            hiddenFileInput.current.click();
        }
    };

    const select = (tag: TagModel) => {
        let index = selectTags.findIndex(e => e.id == tag.id);
        if (index > -1) {
            return;
        }
        else {
            setSelectTags([...selectTags, tag]);
            setTags([]);
        }
    }


    const unselect = (tag: TagModel) => {
        let index = selectTags.findIndex(e => e.id == tag.id);
        if (index > -1) {
            selectTags.splice(index, 1);
            setSelectTags([...selectTags]);
        }
    }

    const [enteredText, setEnteredText] = useState<string>('');
    const onkeydown = (e: React.KeyboardEvent) => {
        if(e.key === 'Enter' && enteredText.length > 0) {
            setSelectTags([...selectTags, new TagModel(0, enteredText)]);
            setEnteredText('');
        }
    }

    const tagChange = (e: BaseSyntheticEvent) => {
        if (e.target.value.length == 0) {
            setTags([]);
            return;
        }
        setEnteredText(e.target.value);
        pictureService.searchTags(enteredText).then(
            (data: any) => {
                setTags(data.data);
            }
        );
    }

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
            <div className={styles.tagZone}>
                <div className={styles.searchTags}>
                    <input
                        type="text"
                        onChange={tagChange}
                        placeholder='теги'
                        onKeyDown={onkeydown}
                    />
                    <div className={styles.searchResult}>
                        {tags.map((tag: TagModel) => (
                            <div
                                key={tag.id}
                                onClick={() => select(tag)}
                            >
                                <p>{tag.name}</p>
                            </div>
                        ))}
                    </div>
                </div>
                <div className={styles.selectedTags}>
                    {selectTags.map((tag, i) => (
                        <div
                            key={i}
                            className={styles.selectedTag}
                            onClick={() => unselect(tag)}
                        >
                            <p>{tag.name}</p>
                        </div>
                    ))}
                </div>
            </div>
            <div className={styles.imgZone}>
                <button
                    className={styles.submitButton}
                    onClick={createImage}
                >
                    Загрузить изображение
                </button>
            </div>
            <div>
                <img
                    src=""
                    alt=""
                    ref={selectImg}
                    className={styles.selectImg}
                />
            </div>
        </div>
    );
}

export default CreateImage; 