import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata, ElementAbstractTerms } from "../models/content";
import { AddTranslationDto } from "../models/dtos";
import { AbstractTerm, UserTermDetails } from "../models/userTerm";

export default class TermStore {

    phraseMode = false;
    phraseTerms: AbstractTerm[] = [];

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

    addTranslation = async (dto: AddTranslationDto) => {
        try {
           await agent.UserTermEndpoints.addTranslation(dto);
           runInAction(() => {
               this.selectedTerm?.translations.push(dto.newTranslation);
           }) 
        } catch (error) {
            
        }
    }

    refreshTerm = (details: UserTermDetails) => {
        console.log(`Refreshing term: ${details.termValue}`);
    
           
        for(let element of this.elementTermMap) {
            for (let term of element[1].abstractTerms) {
                if (term.termValue.toUpperCase() === details.termValue.toUpperCase()) {
                    const idx = term.indexInChunk;
                    const value = term.termValue;
                    term = {...term, ...details};
                    term.termValue = value;
                    term.indexInChunk = idx;
                    console.log('value refreshed');
                    element[1].abstractTerms[idx] = term;
                }
            }
        }

        if (this.selectedTerm?.termValue.toUpperCase() === details.termValue.toUpperCase()) {
            const value = this.selectedTerm.termValue;
            this.selectedTerm = {...this.selectedTerm, ...details};
            this.selectedTerm.termValue = value;
            console.log('value refreshed');
        }
 
    }

    refreshAbstractTerm = (term: AbstractTerm) => {
        if (!term.hasUserTerm) {
            console.log('no refresh needed');
        }
        const details: UserTermDetails = {...term};
        console.log(`Updating details for ${term.termValue}`);
        console.log(details);
        this.refreshTerm(details)
    }

    selectContentByIdAsync = async (id: string) => {
        this.metadataLoaded = false;
        try {
           const newContent = await agent.Content.getContentWithId({contentId: id});
           runInAction(() => {
               this.selectedContent = newContent;
               this.metadataLoaded = true;
               this.elementTermMap.clear();
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
    }
    
}