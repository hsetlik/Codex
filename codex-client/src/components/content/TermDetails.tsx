import React from "react";
import { Container, Header } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";

interface Props {
    term: AbstractTerm
}

export default function TermDetails({term}: Props) {

    return (
        <Container>
            <Header as='h2' content={term.termValue} />
        </Container>
    )

}