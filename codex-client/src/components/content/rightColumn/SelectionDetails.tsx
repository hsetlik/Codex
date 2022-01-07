import { observer } from "mobx-react-lite";
import { Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import AbstractPhraseDetails from "./AbstractPhraseDetails";
import TermDetails from "./AbstractTermDetails";


export default observer(function SelectionDetails() {
    const {contentStore} = useStore();
    const {selectedTerm, phraseMode} = contentStore;
   
    return (
            (selectedTerm === null) ? (
                <Header content="No term selected" />
                ) : (
                    (phraseMode) ? (
                        <AbstractPhraseDetails />
                    ) : (
                        <TermDetails term={selectedTerm} />
                    )
                )
    )
    
    
    

});