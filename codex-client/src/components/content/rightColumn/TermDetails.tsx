import { observer } from "mobx-react-lite";
import { Header } from "semantic-ui-react";
import { AbstractTerm } from "../../../app/models/userTerm";
import UserTermCreator from "./UserTermCreator";
import UserTermDetails from "./UserTermDetails";
import PopTranslations from "./PopTranslations";
import StarButton from "./StarButton";

interface Props {
    term: AbstractTerm
}

export default observer(function TermDetails({term}: Props) {
   
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
                    <PopTranslations term={term} />
                    <UserTermCreator term={term} />
                </div>
            )}
        </div>
    )
})