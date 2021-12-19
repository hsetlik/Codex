import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { TranslationResultDto } from "../models/dtos";
import { Term } from "../models/userTerm";


export default class TranslationStore
{
    translationsLoaded = false;
    currentTermValue: Term = {termValue: '', language: ''}
    currentTranslations: TranslationResultDto[] = [];
    constructor() {
        makeAutoObservable(this);
    }

    prepareForTerm = async (input: Term) => {
        this.translationsLoaded = false;
        this.currentTranslations = [];
        try {
           const newTranslations = await agent.Translate.getTranslations({value: input.termValue, language: input.language});
           runInAction(() => {
               this.currentTranslations = newTranslations;
               this.currentTermValue = input;
               this.translationsLoaded = true;
           }) 
        } catch (error) {
           console.log(error); 
           runInAction(() => {
               this.translationsLoaded = true;
           })
        }
    }
}