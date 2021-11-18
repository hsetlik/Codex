import { AbstractTerm } from "../../app/models/userTerm";
import TrailingCharacterGroup from "./TrailingCharacterGroup";
import "../styles/styles.css";
import BaseTranscriptTerm from "./BaseTranscriptTerm";
import { Container } from "semantic-ui-react";

interface Props {
    term: AbstractTerm
}


export default function TranscriptTerm({term}: Props) {
    if (term.trailingCharacters.length > 0) {
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
}