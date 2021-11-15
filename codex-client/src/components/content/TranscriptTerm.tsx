import React from "react";
import { Grid, Item, Label } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";

interface Props {
    term: AbstractTerm
}

export default function TranscriptTerm({term}: Props) {
    if (term.hasUserTerm) {
        return (
            <Grid.Column  >
                <Label color='teal'>{term.termValue}</Label>
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