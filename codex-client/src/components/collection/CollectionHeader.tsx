import { observer } from "mobx-react-lite";
import { useState } from "react";
import { Button, Icon } from "semantic-ui-react";
import { Collection } from "../../app/models/collection";
import { useStore } from "../../app/stores/store";
import "../styles/collection.css";
import "../styles/flex.css";
import CollectionContent from "./CollectionContent";

interface Props { collection: Collection }
export default observer(function CollectionHeader({ collection }: Props) {
    const [expanded, setExpanded] = useState(true);
    const handleClick = () => {
        console.log('changing expansion...');
        setExpanded(!(expanded!));
        console.log(`Expanded: ${expanded}`);
    }
    const { userStore: { user }, collectionStore: {deleteCollection} } = useStore();
    const isOwned = (collection.creatorUsername === user?.username);
    return (
        <div className="collection-container">
            <div className="hflex-headers" >
               <h2 className="collection-header">{collection.collectionName}</h2> 
               {isOwned && (
                   <Icon name='trash' color="red" link onClick={() => deleteCollection(collection.collectionId)} style={{'order': 3}} />
               )}
            </div>
            <p className="p-description">{collection.description}</p>
            <div>
                <div className="hflex-basic">
                    <h3 className="collection-subhead">{`${collection.contents.length} items`}</h3>
                    <Icon className="collection-subhead" name={(expanded)? 'minus circle' : 'add circle'} link onClick={handleClick}/>
                </div>
                {expanded && collection.contents.map(c => (
                    <CollectionContent content={c} isOwned={isOwned} collectionId={collection.collectionId} />
                ))}
            </div>
        </div>
    )
})