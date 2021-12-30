import { observer } from "mobx-react-lite";
import React from "react";
import { Container } from "semantic-ui-react";
import { ElementAbstractTerms } from "../../../../app/models/content";
import AbstractTermComponent from "../commonReader/term/AbstractTermComponent";

interface Props {
    terms: ElementAbstractTerms
}
export default observer(function CaptionElementDiv({terms}: Props) {

    return (
        <Container>
            {terms.abstractTerms.map(term => (
                <AbstractTermComponent term={term} tag={terms.tag}/>
            ))
            }
        </Container>
    )

})