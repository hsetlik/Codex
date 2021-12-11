
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import TranscriptReader from "../transcript/TranscriptReader";


export default observer(function ContentRoute(){
    const {contentId, index} = useParams();
    const {contentStore: {selectedContentMetadata}} = useStore();
    if (!contentId) {
        return (
            <Header content='Loading...' />
        )
    }
   return(
        <TranscriptReader contentUrl={selectedContentMetadata?.contentUrl!} contentId={contentId!} index={parseInt(index!)}  />
    )
})