import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Header, List } from "semantic-ui-react";
import AddTranslationForm from "./AddTranslationForm";
import RatingButtonGroup from "./RatingButtonGroup"
import { useStore } from "../../../app/stores/store";
import Translation from "./Translation";
import "../../styles/details.css";

//NOTE: this is an observer because its props are from a store object, even though this component doesn't call useStore() itself
export default observer(function UserTermDetails() {
    const {contentStore: {selectedTerm, termTranslationsLoaded: translationsLoaded, loadSelectedTermTranslations}} = useStore();
    useEffect(() => {
        if (!translationsLoaded) {
            loadSelectedTermTranslations();
        }
    }, [translationsLoaded, loadSelectedTermTranslations]);
    return (
            <div>
                <Header as='h3' content='Translations' className="details-h2" />
                <List >
                    {translationsLoaded && selectedTerm!.translations.map(t => (
                       <Translation term={selectedTerm!} value={t} key={t} /> 
                    ))}
                </List>
                <RatingButtonGroup />
                <Header as='h3' content='Add Translation' className="details-h2"/>
                <AddTranslationForm term={selectedTerm!} />
            </div>
    )

})