import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { TranslationResultDto, TranslatorQuery } from "../models/dtos";
import { Term } from "../models/userTerm";


export default class TranslationStore
{
    translationsLoaded = false;
    currentTermValue: Term = {termValue: '', language: ''}
    currentTranslations: TranslationResultDto[] = [];
    reccomendedLoaded = false;
    reccomendedTranslation: TranslationResultDto = {
        value: 'null',
        annotation: 'null'
    }
    constructor() {
        makeAutoObservable(this);
    }
     clear = () => {
         this.reccomendedTranslation = {
             value: 'null',
             annotation: 'null'
         }
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

    loadReccomended = async (query: TranslatorQuery) => {
        this.reccomendedLoaded = false;
        this.clear();
        try {
            const translation = await agent.Translate.getTranslation(query);
            runInAction(() => {
                this.reccomendedTranslation = {
                    value: translation.responseValue,
                    annotation: 'null'
                }
                this.currentTermValue = {
                    termValue: query.queryValue,
                    language: query.queryLanguage
                }
                this.reccomendedLoaded = true;
            } );
        } catch (error) {
           console.log(error);
           runInAction(() => this.reccomendedLoaded = true);
        }
    }
}