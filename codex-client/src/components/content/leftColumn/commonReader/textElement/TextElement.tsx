import { observer } from "mobx-react-lite";
import { ElementAbstractTerms } from "../../../../../app/models/content";
import AbstractTermComponent from "../term/AbstractTermComponent";

interface Props {
    terms: ElementAbstractTerms;
    style?: React.CSSProperties;
  
}


export default observer (function TextElement({terms, style}: Props) {
    return (
        <>
            {terms.abstractTerms.map(term => ( <AbstractTermComponent tag={terms.tag || 'span'} term={term} key={term.indexInChunk + 1} style={style}/> ))}
        </>
    );
})