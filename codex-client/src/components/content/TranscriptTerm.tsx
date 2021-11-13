import React from "react";
import { Container, Item, Label } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";

interface Props {
    term: AbstractTerm
}

export default function TranscriptTerm({term}: Props) {
    if (term.hasUserTerm) {
        return (
            <Item  >
                <Label color='teal'>{term.termValue}</Label>
            </Item>
        )
    } else {
        return (
            <Item >
                <Label color='green'>{term.termValue}</Label>
            </Item>
        )
    }
}