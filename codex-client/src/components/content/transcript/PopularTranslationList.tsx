import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { List, Button } from "semantic-ui-react";
import { PopularTranslationDto, UserTermCreateDto } from "../../../app/api/agent";
import { AbstractTerm } from "../../../app/models/userTerm";
import { useStore } from "../../../app/stores/store";

interface Props {
    term: AbstractTerm
}

export default observer(function PopularTranslationList({term}: Props){
    const {transcriptStore, userStore} = useStore();
    const {createTerm} = userStore;
    const {popTranslationsLoaded, currentPopularTranslations, loadPopularTranslations, selectedTerm} = transcriptStore;
    useEffect(() => {
       if (!popTranslationsLoaded) {
           loadPopularTranslations();
       }
    }, [popTranslationsLoaded, loadPopularTranslations]);

    const createWithTranslation = async (dto: PopularTranslationDto) => {
        const createTermDto: UserTermCreateDto = {
            language: selectedTerm?.language!,
            termValue: selectedTerm?.termValue!,
            firstTranslation: dto.value
        }
        await createTerm(createTermDto);
    }
    if (currentPopularTranslations.length < 1) {
        return (
            <div></div>
        )
    } else {
        return(
            <List>
                <List.Header as='h4' content='Popular Translations: ' className='codex-sub-header'/>
                {currentPopularTranslations.map(tran => (
                    <List.Item key={tran.value}>
                        <Button onClick={() => createWithTranslation(tran)}>{tran.value + ' (' + tran.numInstances + ')'}</Button>
                    </List.Item>
                ))}
            </List>
        )
    }
})