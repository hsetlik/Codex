import { makeAutoObservable, runInAction } from "mobx";
import agent, { PopularTranslationDto} from "../api/agent";
import { AbstractTerm, Term } from "../models/userTerm";


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
           const newTranslations = await agent.TermEndpoints.getPopularTranslations(input);
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