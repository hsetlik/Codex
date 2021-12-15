import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { PopularTranslationDto } from "../models/dtos";
import { Term } from "../models/userTerm";


export default class TranslationStore
{
    translationsLoaded = false;
    currentTermValue: Term = {termValue: '', language: ''}
    currentPopTranslations: PopularTranslationDto[] = [];
    constructor() {
        makeAutoObservable(this);
    }

    prepareForTerm = async (input: Term) => {
        this.translationsLoaded = false;
        try {
           const newTranslations = await agent.TermEndpoints.getPopularTranslations({value: input.termValue, language: input.language});
           runInAction(() => {
               this.currentPopTranslations = newTranslations;
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