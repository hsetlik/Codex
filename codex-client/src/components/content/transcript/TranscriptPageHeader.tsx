import { observer } from "mobx-react-lite";
import React from "react";
import { useNavigate } from "react-router";
import { Link } from "react-router-dom";
import { Header, Segment, Button } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";

interface Props {
    chunkIndex: number
}

export default observer (function TranscriptPageHeader({chunkIndex}: Props) {
    const {transcriptStore} = useStore();
    const {transcriptChunkIds, contentId} = transcriptStore;
    const nextChunkPath = () => {
        return `../content/${contentId}/${chunkIndex + 1}`;
    }

    const prevChunkPath = () => {
        return `../content/${contentId}/${chunkIndex - 1}`;
    }
    
    
    return(
        <Segment>
            <Header>
               
            </Header>
            <Button basic content='Previous' disabled={chunkIndex === 0} as={Link} to={prevChunkPath()} />
            <Button basic content='Next' disabled={chunkIndex >= transcriptChunkIds.length} as={Link} to={nextChunkPath()}/>
        </Segment>
   )
});