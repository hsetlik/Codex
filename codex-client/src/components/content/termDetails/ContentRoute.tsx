
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import TranscriptReader from "../reader/TranscriptReader";


export default observer(function ContentRoute(){
    const {contentId, index} = useParams();
    const {contentStore: {loadSectionById}} = useStore();
    useEffect(() => {
        loadSectionById(contentId!, parseInt(index!))
    }, [contentId, index, loadSectionById])
    if (!contentId) {
        return (
            <Header content='Loading...' />
        )
    }
   return(
        <TranscriptReader contentId={contentId!} index={parseInt(index!)}  />
    )
})