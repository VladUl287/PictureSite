import styles from './Main.module.css';
import { FC, useEffect } from 'react';
import { IState } from '../../models/IState';
import { useDispatch, useSelector } from 'react-redux';
import { getPictures, getPicturesByTag } from '../../redux/actions/pictureActions';
import { AppDispatch } from '../../App';
import { LazyLoadImage } from 'react-lazy-load-image-component';
import { Link, useSearchParams } from 'react-router-dom';

const Main: FC = () => {

    const dispatch: AppDispatch = useDispatch();
    const images = useSelector((state: IState) => state.picture.pictures);
    const [searchParams] = useSearchParams();

    useEffect(() => {
        let tagId = searchParams.get('tag');
        if(tagId) {
            dispatch(getPicturesByTag(+tagId));
        } else {
            dispatch(getPictures());
        }
    }, [searchParams]);
    
    return (
        <div>
            <div className={styles.masonry}>
                {images.map(image => (
                    <div key={image.id} className={styles.masonryItem}>
                        <Link to={"/picture/" + image.id}>
                            <LazyLoadImage
                                src={image.view}
                            />
                        </Link>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default Main;