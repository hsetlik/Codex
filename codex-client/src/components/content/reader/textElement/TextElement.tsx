import { observer } from "mobx-react-lite";
import { ElementAbstractTerms } from "../../../../app/models/content";
import { classNameForElement } from "../../../../app/models/readerStyle";
import AbstractTermComponent from "../term/AbstractTermComponent";

interface Props {
    terms: ElementAbstractTerms;
}


export default observer (function TextElement({terms}: Props) {
    //TODO: figure out how to actually make use of HTML tags
    return (
        <div className={classNameForElement(terms.tag)}>
            {terms.abstractTerms.map(term => ( <AbstractTermComponent tag={terms.tag} term={term} key={term.indexInChunk}/> ))}
        </div>
    );
})