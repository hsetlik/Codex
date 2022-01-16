import { Label } from "semantic-ui-react";
import { AbstractTerm } from "../../../../../app/models/userTerm";
import '../../../../styles/content.css';

interface Props {
    term: AbstractTerm
}

export default function TrailingCharacterGroup({term}: Props) {
    return (
            <Label as="p" className="codex-term-p-t"  >
                {term.trailingCharacters}
            </Label>
    );
}