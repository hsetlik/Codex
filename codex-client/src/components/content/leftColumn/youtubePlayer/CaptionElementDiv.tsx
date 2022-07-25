import { observer } from "mobx-react-lite";
import { Container, Icon } from "semantic-ui-react";
import { ElementAbstractTerms } from "../../../../app/models/content";
import AbstractTermComponent from "../commonReader/term/AbstractTermComponent";

interface Props {
    terms: ElementAbstractTerms,
    isHighlighted: boolean,
    jumpFunction: () => void
}
export default observer(function CaptionElementDiv({terms, isHighlighted, jumpFunction}: Props) {

    return (
        <Container>
            {isHighlighted && (

                <Icon name='arrow circle right'   />
            )}
            {terms.abstractTerms.map(term => (
                <AbstractTermComponent term={term} tag={terms.tag || 'span'} key={term.indexInChunk}/>
            ))
            }
            <Icon onClick={jumpFunction} name='play'  link/>
        </Container>
    )

})