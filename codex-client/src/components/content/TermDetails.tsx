import React from "react";
import { Container, Header } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";
import UserTermCreator from "./UserTermCreator";

interface Props {
    term: AbstractTerm
}

export default function TermDetails({term}: Props) {
    return (
        <Container className="segment">
            <Header as='h2' content={term.termValue} />
            <UserTermCreator />
        </Container>
    )

}