import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata, ElementAbstractTerms } from "../models/content";
import { AbstractTerm, UserTermDetails } from "../models/userTerm";

export default class TermStore {
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

    elements: Map<string, ElementAbstractTerms> = new Map();
    
    constructor() {
        makeAutoObservable(this);
    }

    get allTerms() {
        let terms: AbstractTerm[] = [];
        for(let element of this.elements) {
            terms.concat(element[1].abstractTerms);
        }
        return terms;
    }

    refreshTerm = (details: UserTermDetails) => {
        for(let term of this.allTerms) {
            if (term.termValue.toUpperCase() === details.termValue.toUpperCase()) {
                const caseSensitiveValue = term.termValue;
                term = {...term, ...details};
                term.termValue = caseSensitiveValue;
            }
        }
    }

    refreshAbstractTerm = (term: AbstractTerm) => {
        if (!term.hasUserTerm) {
            console.log('no refresh needed');
        }
        const details: UserTermDetails = {...term};
        this.refreshTerm(details)
    }

    selectContentByIdAsync = async (id: string) => {
        this.metadataLoaded = false;
        try {
           const newContent = await agent.Content.getContentWithId({contentId: id});
           runInAction(() => {
               this.selectedContent = newContent;
               this.metadataLoaded = true;
               this.elements.clear();
           }) 
        } catch (error) {
           console.log(error);
           runInAction(() => { 
               this.metadataLoaded = true;
               this.elements.clear();
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
               this.elements.set(elementText, newElementTerms);
           });
        } catch (error) {
           console.log(error); 
        }
    }
    
}