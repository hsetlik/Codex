import { observer } from "mobx-react-lite";
import { Container } from "semantic-ui-react";
import { ElementAbstractTerms } from "../../../../app/models/content";
import AbstractTermComponent from "../term/AbstractTermComponent";

interface Props {
    terms: ElementAbstractTerms;
}


export default observer (function TextElement({terms}: Props) {
    //TODO: figure out how to actually make use of HTML tags
    return (
        <Container>
            {terms.abstractTerms.map(term => ( <AbstractTermComponent term={term} key={term.indexInChunk}/> ))}
        </Container>
    );
})