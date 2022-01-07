import { Button } from "semantic-ui-react";
import { AbstractTerm } from "../../../../../app/models/userTerm";
import '../../../../styles/content.css';
import { useStore } from "../../../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { getColorForTerm } from "../../../../../app/utilities/colorUtility";
import { classNameForTerm } from "../../../../../app/models/readerStyle";
import { useKeyPressed } from "../../../../../app/common/useKeypress";




interface Props {
    term: AbstractTerm,
    tag: string
}

export default observer(function WordComponent({term, tag}: Props) {
    const {contentStore, phraseStore} = useStore();
    const {selectTerm, selectedTerm} = contentStore;
    const {currentSelectedTerms} = phraseStore;
    const termColor = getColorForTerm(term);
    const className = classNameForTerm(tag, currentSelectedTerms.some(t => t === term) || selectedTerm === term);
    const shiftPressed = useKeyPressed((e) => {
        return e.shiftKey;
    });
    return (
            <Button 
            className={className} 
            onClick={() => selectTerm(term, shiftPressed)}
            style={{background: termColor}}>
                    {term.termValue}
            </Button>
    )
    
});