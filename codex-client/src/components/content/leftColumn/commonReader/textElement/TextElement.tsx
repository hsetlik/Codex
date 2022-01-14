import { observer } from "mobx-react-lite";
import { ElementAbstractTerms } from "../../../../../app/models/content";
import AbstractTermComponent from "../term/AbstractTermComponent";

interface Props {
    terms: ElementAbstractTerms;
  
}


export default observer (function TextElement({terms}: Props) {
    return (
        <>
            {terms.abstractTerms.map(term => ( <AbstractTermComponent tag={terms.tag} term={term} key={term.indexInChunk}/> ))}
        </>
    );
})