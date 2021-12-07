
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Header } from "semantic-ui-react";
import agent from "../../../app/api/agent";
import { useStore } from "../../../app/stores/store";
import TranscriptReader from "../transcript/TranscriptReader";
import {useAsyncEffect} from 'use-async-effect';

export default function ContentRoute(){
    const {contentId, index} = useParams();
    const {contentStore: {selectedContentMetadata, setSelectedContentById, loadSection}} = useStore();
    useEffect(() => {
        if (selectedContentMetadata?.contentId !== contentId) {
            setSelectedContentById(contentId!);
        }
        const parNumber = parseInt(index!);
        if (parNumber > 0) {
            loadSection(selectedContentMetadata?.contentUrl!, parNumber);
        }
    }, [setSelectedContentById, selectedContentMetadata, contentId]);
    if (!contentId) {
        return (
            <Header content='Loading...' />
        )
    }
   return(
        <TranscriptReader contentUrl={selectedContentMetadata?.contentUrl!} contentId={contentId!} index={parseInt(index!)}  />
    )
}