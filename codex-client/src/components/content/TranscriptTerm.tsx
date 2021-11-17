import { Label } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";
import TrailingCharacterGroup from "./TrailingCharacterGroup";
import "../styles/styles.css";

interface Props {
    term: AbstractTerm
}


//note: the userTerm will eventually need a callback function
export function BaseTranscriptTerm({term}: Props) {
    if (term.hasUserTerm) {
        return (
            <Label className="basic-codex-term" >
                {term.termValue}
            </Label>
        )
    } else {
        return (
                <Label className="basic-codex-term">
                     {term.termValue}
                </Label>
        )
    }
}

export  default function TranscriptTerm({term}: Props) {
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