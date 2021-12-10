import { observer } from "mobx-react-lite";
import React from "react";
import { Container, Header } from "semantic-ui-react";
import { AbstractTerm } from "../../../app/models/userTerm";
import UserTermCreator from "./UserTermCreator";
import UserTermDetails from "../termDetails/UserTermDetails";
import PopTranslations from "./PopTranslations";

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
                <div>
                    <PopTranslations term={term} />
                    <UserTermCreator term={term} />
                </div>
            )}
        </Container>
    )

})