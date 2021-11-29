import React from "react";
import { Item, ItemContent } from "semantic-ui-react";
import { AbstractTerm } from "../../../app/models/userTerm";

interface Props {
    term: AbstractTerm
}

export default function TranslationsList({term}: Props) {
    return(
        <Item.Group>
            {term.translations.map(tran => {
                return <ItemContent content={tran} key={tran} />
            })}
        </Item.Group>
    )
}