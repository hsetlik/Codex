import { AbstractTerm } from "../../../app/models/userTerm";
import TrailingCharacterGroup from "./TrailingCharacterGroup";
import "../../styles/styles.css";
import BaseTranscriptTerm from "../termDetails/BaseTranscriptTerm";
import { observer } from "mobx-react-lite";
import LeadingCharacterGroup from "./LeadingCharacterGroup";

interface Props {
    term: AbstractTerm
}


export default observer(function  TranscriptTerm({term}: Props) {
        return (
            <>
                {term.leadingCharacters !== 'none' &&
                    <LeadingCharacterGroup term={term} />
                }
               <BaseTranscriptTerm term={term} />
               {term.trailingCharacters !== 'none' &&
                     <TrailingCharacterGroup term={term} />
               }
            </>
        );
}
)