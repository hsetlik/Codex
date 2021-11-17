import { Button, Grid, Label } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";

interface Props {
    term: AbstractTerm
}

function LabelLengthFor(word: string)
{
    return (word.length) * 24.0;
}

export default function TranscriptTerm({term}: Props) {
    if (term.hasUserTerm) {
        return (
            <Label >
                {term.termValue}
            </Label>
        )
    } else {
        return (
                <Label color='green'>{term.termValue}</Label>
        )
    }
}