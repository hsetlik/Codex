import { Label } from "semantic-ui-react";
import { AbstractTerm } from "../../../../app/models/userTerm";
import "../../../styles/styles.css";

interface Props {
    term: AbstractTerm
}

export default function LeadingCharacterGroup({term}: Props) {
    return (
            <Label as="p" className="leading-character-group">
                {term.leadingCharacters}
            </Label>
    );
}