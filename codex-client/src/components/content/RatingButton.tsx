import { observer } from "mobx-react-lite";
import React from "react";
import { Button } from "semantic-ui-react";
import { AbstractTerm } from "../../app/models/userTerm";

interface Props {
    ratingValue: number,
    term: AbstractTerm
}

export default observer (function RatingButton({ratingValue, term}: Props) {
    const rateTerm = () => {
        
    }

    return (
        <Button basic >

        </Button>
    )
})