import { observer } from "mobx-react-lite";
import React from "react";
import { Container, Icon } from "semantic-ui-react";
import { ElementAbstractTerms } from "../../../../app/models/content";
import AbstractTermComponent from "../commonReader/term/AbstractTermComponent";

interface Props {
    terms: ElementAbstractTerms,
    isHighlighted: boolean
}
export default observer(function CaptionElementDiv({terms, isHighlighted}: Props) {

    return (
        <Container>
            {isHighlighted && (

                <Icon name='arrow circle right' onClick={() => console.log('arrow clicked')} link  />
            )}
            {terms.abstractTerms.map(term => (
                <AbstractTermComponent term={term} tag={terms.tag} key={term.indexInChunk}/>
            ))
            }
        </Container>
    )

})