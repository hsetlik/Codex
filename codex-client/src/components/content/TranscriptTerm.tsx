import { Grid, Label } from "semantic-ui-react";
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
            <Grid.Column color='teal' style={{outerWidth: LabelLengthFor(term.termValue)}} >
                {term.termValue}
            </Grid.Column>
        )
    } else {
        return (
            <Grid.Column >
                <Label color='green'>{term.termValue}</Label>
            </Grid.Column>
        )
    }
}