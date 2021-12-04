
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Header } from "semantic-ui-react";
import agent from "../../../app/api/agent";
import { useStore } from "../../../app/stores/store";
import TranscriptReader from "../transcript/TranscriptReader";

export default function ContentRoute(){
    const {contentId, index} = useParams();
    const {contentStore: {selectedContentMetadata, setSelectedContent, selectedContentUrl}} = useStore();
    useEffect(() => {
        if () {
           
            
        }
    }, [ selectedContentMetadata, index])
    if (!contentId) {
        return (
            <Header content='Loading...' />
        )
    }
   return(
        <TranscriptReader contentUrl={selectedContentMetadata?.contentUrl!} index={parseInt(index!)}  />
    )
}