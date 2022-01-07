import { observer } from "mobx-react-lite";
import { Header } from "semantic-ui-react";
import { AbstractTerm } from "../../../app/models/userTerm";
import UserTermCreator from "./UserTermCreator";
import UserTermDetails from "./UserTermDetails";
import StarButton from "./StarButton";
import ReccomendedTranslation from "./ReccomendedTranslation";

interface Props {
    term: AbstractTerm
}

export default observer(function AbstractTermDetails({term}: Props) {
    return (
        <div>
            {term.hasUserTerm ? (
                <div>
                    <StarButton />
                    <Header as='h2' content={term.termValue} />
                    <UserTermDetails />
                </div>
            ) : (
                <div>
                    <Header as='h2' content={term.termValue} />
                    <ReccomendedTranslation term={{value: term.termValue, language: term.language}} />
                    <UserTermCreator term={term} />
                </div>
            )}
        </div>
    )
})