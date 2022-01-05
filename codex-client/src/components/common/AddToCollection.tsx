import { observer } from "mobx-react-lite";
import React from "react";
import { Dropdown } from "semantic-ui-react";
import { ContentMetadata } from "../../app/models/content";
import { useStore } from "../../app/stores/store";

interface Props {content: ContentMetadata}

export default observer( function AddToCollection({content}: Props) {
    const {collectionStore} = useStore();
    const {currentCollections, addToCollection} = collectionStore;
    const handleChange = (collectionId: string) => {
        addToCollection(collectionId, content);
    }
    return (
        <Dropdown value='Add to Collection' className='button' text="Add to Collection">
            <Dropdown.Menu>
                {currentCollections.map(col => (
                    <Dropdown.Item 
                    key={col.collectionId}
                    onClick={() => handleChange(col.collectionId)}
                    >
                        {col.collectionName}
                    </Dropdown.Item>
                ))}
            </Dropdown.Menu>
        </Dropdown>
    )
})