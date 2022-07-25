import { AbstractTerm } from "../../../../../app/models/userTerm";
import '../../../../styles/word-component.css';

interface Props {
    term: AbstractTerm
}

export default function TrailingCharacterGroup({term}: Props) {
    return (
            <p className="character-group"  >
                {term.trailingCharacters}
            </p>
    );
}