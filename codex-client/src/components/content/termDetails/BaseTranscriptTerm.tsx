import { Button } from "semantic-ui-react";
import { AbstractTerm } from "../../../app/models/userTerm";
import TrailingCharacterGroup from "../transcript/TrailingCharacterGroup";
import "../../styles/styles.css";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { getColorForTerm } from "../../../app/utilities/colorUtility";

interface Props {
    term: AbstractTerm
}

export default observer(function BaseTranscriptTerm({term}: Props) {
    const {contentStore} = useStore();
    const {setSelectedTerm} = contentStore;
    const termColor = getColorForTerm(term);
    if (term.hasUserTerm) {
        return (
            <Button as="p" className="basic-codex-term" onClick={() => setSelectedTerm(term)} style={{background: termColor}}>
                {term.termValue}
            </Button>
        )
    } else {
        return (
            <Button as="p" className="basic-codex-term" onClick={() => setSelectedTerm(term)} style={{background: termColor}} >
                    {term.termValue}
            </Button>
        )
    }
});