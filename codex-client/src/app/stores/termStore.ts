import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata, ElementAbstractTerms } from "../models/content";
import { AddTranslationDto } from "../models/dtos";
import { AbstractPhrase } from "../models/phrase";
import { AbstractTerm, UserTerm } from "../models/userTerm";
import { store } from "./store";

export default class TermStore {

    phraseMode = false;
    phraseTerms: AbstractTerm[] = [];
    selectedAbstractPhrase: AbstractPhrase | null = null;

    selectedTerm: AbstractTerm | null = null;



    metadataLoaded = false;
    selectedContent: ContentMetadata = {
        contentId: 'null',
        contentUrl: 'null',
        videoId: 'null',
        audioUrl: 'null',
        contentType: 'null',
        contentName: 'null',
        dateAdded: 'null',
        language:'null',
        bookmark: 0,
        numSections: 0
    }

    elementTermMap: Map<string, ElementAbstractTerms> = new Map();
    
    constructor() {
        makeAutoObservable(this);
    }

    get allTerms() {
        let terms: AbstractTerm[] = [];
        for(let element of this.elementTermMap) {
            terms = terms.concat([...element[1].abstractTerms]);
        }
        return terms;
    }

    get allElements() {
        const elements: ElementAbstractTerms[] = [...this.elementTermMap.values()];
        return elements;
    }

    loadSelectedTranslations = async () => {
        try {
           if (this.selectedTerm && this.selectedTerm.translations.length < 1) {
            const newTranslations = await agent.UserTermEndpoints.getTranslations({userTermId: this.selectedTerm.userTermId});
            runInAction(() => {
                if (this.selectedTerm)
                    this.selectedTerm.translations = newTranslations.map(t => t.userValue);
            });
           } 
        } catch (error) {
            console.log(error);
            
        }
    }

    deleteTranslation = (value: string) => {
        if (this.selectedTerm?.translations) {
            this.selectedTerm.translations = this.selectedTerm.translations.filter(word => word !== value);
        }
    }

    addTranslation = async (dto: AddTranslationDto) => {
        try {
           await agent.UserTermEndpoints.addTranslation(dto);
           runInAction(() => {
               this.selectedTerm?.translations.push(dto.newTranslation);
           }) 
        } catch (error) {
            
        }
    }

    refreshTerm = (details: UserTerm) => {
        console.log(`Refreshing terms with value ${details.termValue}`);
        for(let element of this.elementTermMap)
        {
            const terms = element[1];
            let newTerms: AbstractTerm[] = [] 
            for (let term of terms.abstractTerms) {
                if (term.termValue.toUpperCase() === details.termValue.toUpperCase()) {
                    console.log(`found match with value ${term.termValue} at index ${term.indexInChunk}`);
                    const newTerm: AbstractTerm = {
                        termValue: term.termValue,
                        language: term.language,
                        trailingCharacters: term.trailingCharacters,
                        leadingCharacters: term.leadingCharacters,
                        hasUserTerm: true,
                        timesSeen: details.timesSeen,
                        rating: details.rating,
                        easeFactor: details.easeFactor,
                        translations: [],
                        indexInChunk: term.indexInChunk,
                        userTermId: details.userTermId,
                        starred: details.starred
                    }
                    if (this.selectedTerm === term) {
                        runInAction(() => this.selectedTerm = newTerm);
                        this.loadSelectedTranslations();
                    }
                    term = newTerm;
                    console.log(term);
                }
                newTerms.push(term);
            }
            runInAction(() => element[1].abstractTerms = newTerms);
        }
    }

    selectContentByIdAsync = async (id: string) => {
        this.metadataLoaded = false;
        try {
           const newContent = await agent.Content.getContentWithId({contentId: id});
           await agent.Account.setLastStudiedLanguage({language: newContent.language});
           runInAction(() => {
               this.selectedContent = newContent;
               console.log(`New selected content has name: ${newContent.contentName}`);
               this.metadataLoaded = true;
               this.elementTermMap.clear();
               // also clear the elements in videoStore and/or articleStore
               store.videoStore.reset();
           }) 
        } catch (error) {
           console.log(error);
           runInAction(() => { 
               this.metadataLoaded = true;
               this.elementTermMap.clear();
            });
        }
    } 

    loadElementAsync = async (elementText: string, tag?: string) => {
        try {
           const newElementTerms = await agent.Content.abstractTermsForElement({
               elementText: elementText,
               tag: tag || 'null',
               contentUrl: this.selectedContent.contentUrl,
               language: this.selectedContent.language
           });
           runInAction(() => {
               this.elementTermMap.set(elementText, newElementTerms);
           });
        } catch (error) {
           console.log(error); 
        }
    }

    parentElementOf = (term: AbstractTerm) => {
        for(let el of this.allElements) {
            if (el.abstractTerms.some(at => at === term))
                return el;
        }
        return null;
    }

    selectTerm = (term: AbstractTerm, shiftDown?: boolean) => {
        if (shiftDown && this.selectedTerm !== null) {
            const el = this.parentElementOf(term)
            if (el === this.parentElementOf(this.selectedTerm)) {
                const aIdx = this.selectedTerm.indexInChunk;
                const bIdx = term.indexInChunk;
                const start = (aIdx > bIdx) ? bIdx : aIdx;
                const end = (aIdx <= bIdx) ? bIdx : aIdx;
                this.phraseTerms = el!.abstractTerms.slice(start, end + 1);
                this.phraseMode = true;
            }
        } else {
            this.selectedTerm = term;
            this.phraseMode = false;
            this.phraseTerms = [];
        }
        this.loadSelectedTranslations();
    }

    updatePhraseAsync = async () => {
        try {
           let phraseValue = '';
           const lastTerm = this.phraseTerms[this.phraseTerms.length - 1];
           for(let term of this.phraseTerms) {
               if (term.leadingCharacters !== 'none')
                phraseValue += term.leadingCharacters;
               phraseValue += term.termValue;
               if (term.trailingCharacters !== 'none')
                phraseValue += term.trailingCharacters;
               if (term !== lastTerm)
                phraseValue += ' ';
           }
           const newAbstractPhrase = await agent.PhraseAgent.getAbstractPhrase({value: phraseValue, language: this.selectedContent.language});
           runInAction(() => {
               this.selectedAbstractPhrase = newAbstractPhrase;
           })
        } catch (error) {
            
        }
    }


    
}