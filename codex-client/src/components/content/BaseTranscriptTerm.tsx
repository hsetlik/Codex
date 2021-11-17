import { Button } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";
import TrailingCharacterGroup from "./TrailingCharacterGroup";
import "../styles/styles.css";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

interface Props {
    term: AbstractTerm
}

export default observer(function BaseTranscriptTerm({term}: Props) {
    const {transcriptStore} = useStore();
    const {selectedTerm} = transcriptStore;
    if (term.hasUserTerm) {
        return (
            <Button as="p" className="basic-codex-term" onClick={() => transcriptStore.setSelectedTerm(term)}>
                {term.termValue}
            </Button>
        )
    } else {
        return (
            <Button as="p" className="basic-codex-term" onClick={() => transcriptStore.setSelectedTerm(term)}>
                    {term.termValue}
            </Button>
        )
    }
});