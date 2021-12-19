import { observer } from "mobx-react-lite";
import React from "react";
import { useEffect } from "react";
import { Segment, Button } from "semantic-ui-react";
import { PopularTranslationDto } from "../../../app/models/dtos";
import { Term } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";

interface Props {
    term: Term;
}

export default observer(function PopTranslations({term}: Props){
    const {translationStore: {currentPopTranslations, currentTermValue, prepareForTerm}, userStore: {createTerm}} = useStore();
    useEffect(() => {
        if (currentTermValue.termValue !== term.termValue) {
            prepareForTerm({termValue: term.termValue, language: term.language});
        }
      
    }, [term, currentTermValue, prepareForTerm])
    const createWithTranslation  = async (translation: PopularTranslationDto) => {
        await createTerm({termValue: term.termValue, language: term.language, firstTranslation: translation.value});
    }
    return(
        <div>{ currentPopTranslations.length > 0 &&
            <Segment>
                {currentPopTranslations.map(tran => (
                    <Button as='h4' content={tran.value} key={tran.value} onClick={() => createWithTranslation(tran)} />
                ))
                }
            </Segment> 
            } 
        </div>
   )
})