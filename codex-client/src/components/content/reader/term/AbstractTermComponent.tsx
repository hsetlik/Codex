import { AbstractTerm } from "../../../../app/models/userTerm";
import TrailingCharacterGroup from "./TrailingCharacterGroup";
import BaseTranscriptTerm from "../BaseTranscriptTerm";
import "../../../styles/styles.css";
import { observer } from "mobx-react-lite";
import LeadingCharacterGroup from "./LeadingCharacterGroup";

interface Props {
    term: AbstractTerm
}

export default observer(function  AbstractTermComponent({term}: Props) {
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