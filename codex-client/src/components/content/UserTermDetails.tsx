import { observer } from "mobx-react-lite";
import React from "react";
import { Container, Header } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";

interface Props {
    term: AbstractTerm
}

//NOTE: this is an observer because its props are from a store object, even though this component doesn't call useStore() itself
export default observer(function UserTermDetails({term}: Props) {

    return (
        <Container className="segment">
            <Header as='h2' content={term.termValue} />
            { term.translations.map(tran => {
                return <Header as='h4' content={tran} key={tran} />
            })
            }
        </Container>
    )

})