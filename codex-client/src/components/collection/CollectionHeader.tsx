import { observer } from "mobx-react-lite";
import React from "react";
import { useState } from "react";
import { Container, Header, Icon } from "semantic-ui-react";
import { Collection } from "../../app/models/collection";
import { useStore } from "../../app/stores/store";
import CollectionContent from "./CollectionContent";

interface Props {collection: Collection}
export default observer( function CollectionHeader({collection}: Props) {
    const [expanded, setExpanded] = useState(true);
    const handleClick = () => {
        console.log('changing expansion...');
        setExpanded(!(expanded!));
        console.log(`Expanded: ${expanded}`);
    }
    const {userStore: {user}} = useStore();
    const isOwned = (collection.creatorUsername === user?.username);
    return (
        <Container>
            <Header>
                <Header>{collection.collectionName}</Header>
                <Header as='h3'>{`${collection.contents.length} items`}</Header>
            </Header>
            <Icon name={(expanded) ? 'minus circle' : 'add circle'} onClick={handleClick} link />
            {expanded && collection.contents.map(con => (
                    <CollectionContent 
                    content={con} 
                    key={con.contentId} 
                    collectionId={collection.collectionId} 
                    isOwned={isOwned} />
            ))}
        </Container>
    )
})