import { observer } from "mobx-react-lite";
import React from "react";
import { useEffect } from "react";
import { Segment, Button } from "semantic-ui-react";
import { Term } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";

interface Props {
    term: Term;
}

export default observer(function PopTranslations({term}: Props){
    const {translationStore: {currentPopTranslations, currentTermValue, prepareForTerm}} = useStore();
    useEffect(() => {
        if (currentTermValue.termValue !== term.termValue) {
            prepareForTerm({termValue: term.termValue, language: term.language});
        }
      
    }, [term, currentTermValue, prepareForTerm])
    return(
        <div>{ currentPopTranslations.length > 0 &&
            <Segment>
                {currentPopTranslations.map(tran => (
                    <Button as='h4' content={tran.value} key={tran.value} />
                ))
                }
            </Segment> 
            } 
        </div>
   )
})