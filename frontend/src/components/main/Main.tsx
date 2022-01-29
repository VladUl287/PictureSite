import styles from './Main.module.css';
import { FC, useEffect } from 'react';
import { IState } from '../../models/IState';
import { useDispatch, useSelector } from 'react-redux';
import { getBooks } from '../../redux/actions/pictureActions';
import { AppDispatch } from '../../App';

const Main: FC = () => {

    const dispatch: AppDispatch = useDispatch();
    const images = useSelector((state: IState) => state.picture.pictures);

    useEffect(() => {
        dispatch(getBooks());
    }, []);

    return (
        <div className={styles.masonry}>
            {images.map(image => (
                <div className={styles.masonryItem}>
                    <img key={image.id} src={image.view} />
                </div>
            ))}
            {images.reverse().map(image => (
                <div className={styles.masonryItem}>
                    <img key={image.id} src={image.view} />
                </div>
            ))}
        </div>
    );
}

export default Main;