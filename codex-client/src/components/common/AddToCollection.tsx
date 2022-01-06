import { observer } from "mobx-react-lite";
import { useState } from "react";
import { Button, Popup, PopupContent, List, Icon, ListContent } from "semantic-ui-react";
import { Collection } from "../../app/models/collection";
import { ContentMetadata } from "../../app/models/content";
import { useStore } from "../../app/stores/store";
import CollectionCreateForm from "../collection/CollectionCreateForm";

interface Props {content: ContentMetadata}

export default observer(function AddToCollection({content}: Props) {
    const {collectionStore, userStore} = useStore();
    const {currentCollections, addToCollection, removeFromCollection} = collectionStore;
    const username = userStore.user?.username || 'null';
    const [creatingNew, setCreatingNew] = useState(false);
    const collectionArray = (): Collection[] => {
        let output: Collection[] = [];
        currentCollections.forEach((value: Collection, key: string) => {
            if (value.creatorUsername === username) {
                output.push(value);
            }
        })
        return output;
    }
    const containsContent = (collectionId: string, content: ContentMetadata): boolean => {
        return currentCollections.get(collectionId)!.contents.some(c => c.contentId === content.contentId);
    }

    const handleCreateClick = () => {
        setCreatingNew(!(creatingNew));
    }

 return (
    <Popup
        openOnTriggerClick={true}
        openOnTriggerMouseEnter={false}
        closeOnTriggerMouseLeave={false}
        trigger={
            <Button content='Add to Collection' />
        }
    >
        <PopupContent>
            <List>
                {!creatingNew && collectionArray().map(col => (
                    <List.Item key={col.collectionId} >
                        {containsContent(col.collectionId, content) ? (
                            <Icon name='minus circle' color='red' link onClick={() => removeFromCollection(col.collectionId, content)} />

                        ) : (
                            <Icon name='add circle' color='green' link onClick={() => addToCollection(col.collectionId, content)} />
                        )}
                        <ListContent>{col.collectionName}</ListContent>
                    </List.Item>
                ))}
                {creatingNew && (
                    <CollectionCreateForm contentUrl={content.contentUrl} />
                )}
                <List.Item key='toggleCreateMode' className="label" >
                    <Icon name='add square' color="blue" link onClick={handleCreateClick} />
                    <ListContent >Create new collection</ListContent>
                </List.Item>
            </List>
        </PopupContent>
    </Popup>
 )
})