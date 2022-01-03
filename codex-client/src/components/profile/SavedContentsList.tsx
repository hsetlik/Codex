import { observer } from "mobx-react-lite";
import React from "react";
import { Header, Loader, Segment } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import SavedContentHeader from "./SavedContentHeader";


export default observer( function SavedContentsList() {
    const {contentStore: {savedContents, savedContentsLoaded}} = useStore();
    if (!savedContentsLoaded) {
        return (
            <Loader active={true} />
        )
    }
    return (

        <div>
            <Header as='h2' content='Saved Contents' />
            <Segment>
            {savedContents.map(c => (
                <SavedContentHeader dto={c} key={c.savedContentId} />
            ))}
            </Segment>
        </div>
    )
})