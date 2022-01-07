import { Button } from "semantic-ui-react";
import { AbstractTerm } from "../../../../../app/models/userTerm";
import '../../../../styles/content.css';
import { useStore } from "../../../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { getColorForTerm } from "../../../../../app/utilities/colorUtility";
import { classNameForTerm } from "../../../../../app/models/readerStyle";
import { useKeyPressed } from "../../../../../app/common/useKeyPressed";


interface Props {
    term: AbstractTerm,
    tag: string
}

export default observer(function WordComponent({term, tag}: Props) {
    const {contentStore} = useStore();
    const {selectTerm, selectedTerm, phraseTerms} = contentStore;
    const termColor = getColorForTerm(term);
    const className = classNameForTerm(tag, selectedTerm === term || phraseTerms.some(t => t === term));
    const shiftDown = useKeyPressed((e) => {
        return e.shiftKey;
    })
    return (
        <Button className={className} onClick={() => selectTerm(term, shiftDown)} style={{background: termColor}}>
            {term.termValue}
        </Button>
    )
  
});