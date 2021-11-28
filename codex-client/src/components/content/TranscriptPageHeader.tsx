import { observer } from "mobx-react-lite";
import React from "react";
import { Header, Segment, Button } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";

export default observer (function TranscriptPageHeader() {
    const {transcriptStore: {currentChunkIndex, transcriptChunkIds, advanceChunk, previousChunk}} = useStore();
    return(
        <Segment>
            <Header>
                Chunk {currentChunkIndex + 1} of {transcriptChunkIds.length}
            </Header>
            <Button basic content='Previous' onClick={previousChunk} disabled={currentChunkIndex === 0} />
            <Button basic content='Next' onClick={advanceChunk} disabled={currentChunkIndex === transcriptChunkIds.length - 1} />
        </Segment>
   )
});