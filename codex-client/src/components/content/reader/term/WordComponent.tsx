import { Button } from "semantic-ui-react";
import { AbstractTerm } from "../../../../app/models/userTerm";
import "../../../styles/styles.css";
import { useStore } from "../../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { getColorForTerm } from "../../../../app/utilities/colorUtility";
import { classNameForTerm } from "../../../../app/models/readerStyle";

interface Props {
    term: AbstractTerm,
    tag: string
}

export default observer(function WordComponent({term, tag}: Props) {
    const {contentStore} = useStore();
    const {setSelectedTerm} = contentStore;
    const termColor = getColorForTerm(term);
    const className = classNameForTerm(tag);
    if (term.hasUserTerm) {
        return (
            <Button className={className} onClick={() => setSelectedTerm(term)} style={{background: termColor}}>
                {term.termValue}
            </Button>
        )
    } else {
        return (
            <Button className={className} onClick={() => setSelectedTerm(term)} style={{background: termColor}} >
                    {term.termValue}
            </Button>
        )
    }
});