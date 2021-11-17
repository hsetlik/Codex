import React from "react";
import { Container, Header } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";

interface Props {
    term: AbstractTerm
}

export default function UserTermDetails({term}: Props) {

    return (
        <Container className="segment">
            <Header as='h2' content={term.termValue} />
            { term.translations.map(tran => {
                return <Header as='h4' content={tran} />
            })
            }
        </Container>
    )

}