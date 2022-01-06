import { observer } from "mobx-react-lite";
import { useState } from "react";
import { Dropdown, ListContent } from "semantic-ui-react";
import { getCollectionsArray } from "../../app/models/collection";
import { ContentMetadata } from "../../app/models/content";
import { useStore } from "../../app/stores/store";
import CollectionCreateForm from "../collection/CollectionCreateForm";

interface Props {content: ContentMetadata}

export default observer( function AddToCollection({content}: Props) {
    const {collectionStore, userStore: {user}} = useStore();
    const {currentCollections, addToCollection} = collectionStore;
    const [creatingNew, setCreatingNew] = useState(false);
    //TODO: logic to make sure that only accessible collections are available
    const handleChange = (collectionId: string) => {
        addToCollection(collectionId, content);
    }
    const createClick = () => {
        setCreatingNew(!(creatingNew!));
    }
    var collectionsArray = getCollectionsArray(currentCollections);
    collectionsArray = collectionsArray.filter(cl => cl.creatorUsername === user?.username);
    return (
        <Dropdown value='Add to Collection' className='button' text="Add to Collection">
           <Dropdown.Menu>
                {creatingNew && (
                    <CollectionCreateForm contentUrl={content.contentUrl} key='createForm' />
                )}
                {!(creatingNew) && collectionsArray.map(col => (
                    <>
                        <Dropdown.Item key={content.contentId + col.collectionId} onClick={() => handleChange(col.collectionId)} >
                           <ListContent key={col.collectionId}>{col.collectionName}</ListContent>
                        </Dropdown.Item>
                        <Dropdown.Item key="createNew" onClick={createClick}>
                            <ListContent key={col.collectionId}>New Collection</ListContent>
                        </Dropdown.Item>
                    </>
                ))}
            </Dropdown.Menu>
        </Dropdown>
    )
})