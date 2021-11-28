import { AbstractTerm } from "../../app/models/userTerm";
import TrailingCharacterGroup from "./TrailingCharacterGroup";
import "../styles/styles.css";
import BaseTranscriptTerm from "./BaseTranscriptTerm";
import { observer } from "mobx-react-lite";

interface Props {
    term: AbstractTerm
}


export default observer(function  TranscriptTerm({term}: Props) {
    if (term.trailingCharacters.length) {
        return (
            <>
               <BaseTranscriptTerm term={term} />
               <TrailingCharacterGroup term={term} />
            </>
        )
    }
    else {
        return <BaseTranscriptTerm term={term} />
    }
})