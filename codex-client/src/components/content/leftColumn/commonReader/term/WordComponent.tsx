import { AbstractTerm } from "../../../../../app/models/userTerm";
import { useStore } from "../../../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { getColorForTerm } from "../../../../../app/utilities/colorUtility";
import { useKeyPressed } from "../../../../../app/common/useKeyPressed";
import '../../../../styles/word-component.css';

interface Props {
    term: AbstractTerm,
    style?: React.CSSProperties,
}



export default observer(function WordComponent({term, style}: Props) {
    const {termStore} = useStore();
    const {selectTerm, selectedTerm, phraseTerms} = termStore;
    const termColor = getColorForTerm(term);
    const selected = (selectedTerm === term || phraseTerms.some(t => t === term));
    const shiftDown = useKeyPressed((e) => {
        return e.shiftKey;
    })

    const addedStyle: React.CSSProperties = (selected) ? {
        background: termColor,
        borderStyle: 'solid',
        alignContent: 'center'

    } : {
        background: termColor
    };
    const mergedStyle: React.CSSProperties = {...addedStyle, ...style};
    if (term.termValue.length < 1)
        return (<span></span>)
    return (
        <button className={'word-component'} onClick={() => selectTerm(term, shiftDown)} style={mergedStyle} >
           {term.termValue}
        </button>
    )
  
});