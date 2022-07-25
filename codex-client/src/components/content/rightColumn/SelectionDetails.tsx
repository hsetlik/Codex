import { observer } from "mobx-react-lite";
import { Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import AbstractPhraseDetails from "./AbstractPhraseDetails";
import TermDetails from "./TermDetails";
import '../../styles/content-frame.css';
import '../../styles/details.css';
import { CssPallette } from "../../../app/common/uiColors";


export default observer(function SelectionDetails() {
    const {termStore} = useStore();
    const {selectedTerm, phraseMode} = termStore; 
    return (
        <div className="details-container">
            {(selectedTerm === null) ? (
                <Header content="No term selected" className="details-h2" />
                ) : (
                    (phraseMode) ? (
                        <div className='deatils-div'style={CssPallette.Surface} >
                            <AbstractPhraseDetails />
                        </div>
                    ) : (
                        <div className='details-div' style={CssPallette.Surface} >
                            <TermDetails term={selectedTerm} />
                        </div>
                    )
                )}
        </div>
            
    )
    
    
    

});