import { observer } from "mobx-react-lite";
import React from "react";
import { Link } from "react-router-dom";
import { Button, Header, Segment } from "semantic-ui-react";
import { ContentMetadata } from "../../app/models/content";
import { useStore } from "../../app/stores/store";

interface Props {
    content: ContentMetadata,
    collectionId: string,
    isOwned: boolean
}

export default observer(function CollectionContent({content, collectionId, isOwned}: Props) {
    const contentPath = `../content/${content.contentId}/${content.bookmark}`;
    const {collectionStore: {removeFromCollection}} = useStore();
    const handleRemove = () => {
        removeFromCollection(collectionId, content);
    }

    return(
        <Segment>
            <Header as={Link} to={contentPath} content={content.contentName} />
            { isOwned && (
                <Button basic floated='right' onClick={handleRemove} >Remove</Button>
            )}
        </Segment>
    )
})