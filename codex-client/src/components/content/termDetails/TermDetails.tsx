import { observer } from "mobx-react-lite";
import React from "react";
import { Container, Header } from "semantic-ui-react";
import { AbstractTerm } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";
import UserTermCreator from "./UserTermCreator";
import UserTermDetails from "../termDetails/UserTermDetails";

interface Props {
    term: AbstractTerm
}



export default observer(function TermDetails({term}: Props) {
   
    return (
        <Container className="segment">
            <Header as='h2' content={term.termValue} />
            {term.hasUserTerm ? (
                <UserTermDetails />
            ) : (
                <UserTermCreator term={term} />
            )}
        </Container>
    )

})