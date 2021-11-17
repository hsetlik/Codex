import { Label } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";
import TrailingCharacterGroup from "./TrailingCharacterGroup";
import "../styles/styles.css";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import BaseTranscriptTerm from "./BaseTranscriptTerm";

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