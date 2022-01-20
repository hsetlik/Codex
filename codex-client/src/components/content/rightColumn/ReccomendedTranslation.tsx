import { observer } from "mobx-react-lite";
import React from "react";
import { useEffect } from "react";
import { Button } from "semantic-ui-react";
import { CssPallette } from "../../../app/common/uiColors";
import { TermDto, TranslationResultDto, TranslatorQuery } from "../../../app/models/dtos";
import { useStore } from "../../../app/stores/store";
import '../../styles/details.css';

interface Props {
    term: TermDto
}


export default observer(function RecommendedTranslation({term}: Props) {
    const {userStore, translationStore} = useStore();
    const {user, createTerm} = userStore;
    const {loadReccomended, reccomendedTranslation, reccomendedLoaded, currentTermValue} = translationStore;
    
    useEffect(() => {
        const tQuery: TranslatorQuery = {
            queryLanguage: term.language,
            queryValue: term.value,
            responseLanguage: user!.nativeLanguage
        }
        if (tQuery.queryValue !== currentTermValue.termValue) {
            console.log(`Query: ${tQuery.queryValue} Current: ${currentTermValue.termValue}`);
            loadReccomended(tQuery);
        }
    }, [loadReccomended, term, user, currentTermValue]);

    const createWithTranslation  = async (translation: TranslationResultDto) => {
        await createTerm({termValue: term.value, language: term.language, firstTranslation: translation.value});
    }
    if (!reccomendedLoaded) {
        return (
            <div>

            </div>
        )
    }

    return (
            <Button as='h4'
            className="suggestion-button" 
            style={CssPallette.Secondary}
            content={reccomendedTranslation.value} 
            key={reccomendedTranslation.value} 
            onClick={() => createWithTranslation(reccomendedTranslation)}
             />
    )
})