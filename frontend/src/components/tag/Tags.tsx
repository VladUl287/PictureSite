import { FC } from "react";
import styles from "./Tags.module.css";
import { TagModel } from "../../models/TagModel";

const Tags: FC<TagProps> = (props: TagProps) => {

    return (
        <div className={styles.selectedTags}>
            {props.tags.map((tag, i) => (
                <div
                    key={i}
                    className={styles.selectedTag}
                    onClick={() => props.clickHandle(tag)}
                >
                    <p>{tag.name}</p>
                </div>
            ))}
        </div>
    );
}

type TagProps = {
    tags: TagModel[],
    clickHandle: Function
}

export default Tags;