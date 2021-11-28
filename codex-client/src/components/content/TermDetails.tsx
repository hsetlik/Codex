import { observer } from "mobx-react-lite";
import React from "react";
import { Container, Header } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";
import UserTermCreator from "./UserTermCreator";
import UserTermDetails from "./UserTermDetails";

interface Props {
    term: AbstractTerm
}



export default observer(function TermDetails({term}: Props) {
    return (
        <Container className="segment">
            <Header as='h2' content={term.termValue} />
            {term.hasUserTerm ? (
                <UserTermDetails term={term} />
            ) : (
                <UserTermCreator term={term} />
            )}
        </Container>
    )

})