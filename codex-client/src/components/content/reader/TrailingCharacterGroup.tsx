import { Label } from "semantic-ui-react";
import { AbstractTerm } from "../../../app/models/userTerm";
import "../../styles/styles.css"

interface Props {
    term: AbstractTerm
}

export default function TrailingCharacterGroup({term}: Props) {
    return (
            <Label as="p" className="trailing-character-group">
                {term.trailingCharacters}
            </Label>
    );
}