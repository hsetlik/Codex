import { observer } from "mobx-react-lite";
import React from "react";
import { AbstractTerm, AbstractToUserTermDetails } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";
import '../../styles/details.css';

interface Props {
    ratingValue: number,
    term: AbstractTerm
}

export default observer (function RatingButton({ratingValue, term}: Props) {
    const {userStore: {updateUserTerm}} = useStore();
    const rateTerm = () => {
        var userTerm = AbstractToUserTermDetails(term);
        userTerm.rating = ratingValue;
        console.log(userTerm);
        updateUserTerm(userTerm);
        console.log(`New Rating: ${ratingValue}`);
    }
    return (
        <button onClick={rateTerm} className='rating-button'>
            {ratingValue}
        </button>
    )
})