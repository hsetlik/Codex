import { observer } from "mobx-react-lite";
import { Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import AbstractPhraseDetails from "./AbstractPhraseDetails";
import TermDetails from "./TermDetails";
import '../../styles/content-frame.css';
import '../../styles/details.css';


export default observer(function SelectionDetails() {
    const {contentStore} = useStore();
    const {selectedTerm, phraseMode} = contentStore; 
    return (
        <div className="details-container">
            {(selectedTerm === null) ? (
                <Header content="No term selected" className="details-h2" />
                ) : (
                    (phraseMode) ? (
                        <AbstractPhraseDetails />
                    ) : (
                        <TermDetails term={selectedTerm} />
                    )
                )}
        </div>
            
    )
    
    
    

});