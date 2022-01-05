import { observer } from "mobx-react-lite";
import React from "react";
import { Header } from "semantic-ui-react";
import { Collection } from "../../app/models/collection";

interface Props {collection: Collection}
export default observer( function CollectionHeader({collection}: Props) {

    return (
        <div>
            <Header>{collection.collectionName}</Header>
        </div>
    )
})