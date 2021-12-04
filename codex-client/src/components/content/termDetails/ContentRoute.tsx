
import { useParams } from "react-router-dom";
import { Header } from "semantic-ui-react";
import TranscriptReader from "../transcript/TranscriptReader";

export default function ContentRoute(){
    const {contentName, index} = useParams();
    if (!contentName) {
        return (
            <Header content='Loading...' />
        )
    }
   return(
        <TranscriptReader contentName={contentName} index={parseInt(index!)}  />
    )
}