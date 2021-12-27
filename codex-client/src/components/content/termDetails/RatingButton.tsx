import { observer } from "mobx-react-lite";
import React from "react";
import { Button } from "semantic-ui-react";
import { AbstractTerm, AbstractToUserTermDetails } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";

interface Props {
    ratingValue: number,
    term: AbstractTerm
}

export default observer (function RatingButton({ratingValue, term}: Props) {
    const {userStore: {updateUserTerm}} = useStore();
    const rateTerm = () => {
        var userTerm = AbstractToUserTermDetails(term);
        userTerm.rating = ratingValue;
        updateUserTerm(userTerm);
        console.log(`New Rating: ${ratingValue}`);
    }
    return (
        <Button basic onClick={rateTerm} active={ratingValue === term.rating} style={{}}>
            {ratingValue}
        </Button>
    )
})