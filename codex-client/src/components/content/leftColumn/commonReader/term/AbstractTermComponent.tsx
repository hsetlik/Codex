import { AbstractTerm } from "../../../../../app/models/userTerm";
import TrailingCharacterGroup from "./TrailingCharacterGroup";
import { observer } from "mobx-react-lite";
import LeadingCharacterGroup from "./LeadingCharacterGroup";
import WordComponent from "./WordComponent";

interface Props {
    term: AbstractTerm,
    tag: string,
    style?: React.CSSProperties
}

export default observer(function  AbstractTermComponent({term, style}: Props) {
        return (
        <>
            {term.leadingCharacters !== 'none' &&
                <LeadingCharacterGroup term={term} />
            }
            <WordComponent term={term} style={style} />
            {term.trailingCharacters !== 'none' &&
                <TrailingCharacterGroup term={term} />
            }
        </>
    );
}
)