
import { useParams } from "react-router-dom";
import { Header } from "semantic-ui-react";
import TranscriptReader from "../transcript/TranscriptReader";

export default function ContentRoute(){
    const {id, index} = useParams();
    if (!id) {
        return (
            <Header content='Loading...' />
        )
    }
   return(
        <TranscriptReader contentUrl={id} index={parseInt(index!)}  />
    )
}