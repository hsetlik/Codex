import { observer } from "mobx-react-lite";
import React from "react";
import { CssPallette } from "../../../app/common/uiColors";
import { AbstractTerm, UserTermDetails } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";
import '../../styles/details.css';

interface Props {
    ratingValue: number,
    term: AbstractTerm
}

export default observer (function RatingButton({ratingValue, term}: Props) {
    const {userStore: {updateUserTerm}} = useStore();
    const rateTerm = () => {
        const userTerm: UserTermDetails = {...term};
        userTerm.rating = ratingValue;
        console.log(userTerm);
        updateUserTerm(userTerm);
        console.log(`New Rating: ${ratingValue}`);
    }
    const buttonStyle = (term.rating === ratingValue) ? CssPallette.SecondaryLight : CssPallette.Secondary;
    return (
        <button onClick={rateTerm} className='rating-button' style={buttonStyle}>
            {ratingValue}
        </button>
    )
})