import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata, ContentPageHtml, ElementAbstractTerms, TextElement } from "../models/content";
import { AbstractTerm, UserTermDetails } from "../models/userTerm";
import { store } from "./store";

export default class HtmlStore {
    htmlLoaded = false;
    currentPageHtml: ContentPageHtml | null = null;
    currentPageContent: ContentMetadata | null = null;
    currentElementsMap = new Map<string, ElementAbstractTerms | null>();
    constructor() {
        makeAutoObservable(this);
    }

    loadPage = async (contentId: string) => {
        this.htmlLoaded = false;
        try {
            console.log(`Loading page with ID: ${contentId}`);
            const content = await agent.Content.getContentWithId({contentId: contentId});
            console.log(`Found content with ID: ${content.contentId} and URL ${content.contentId}`);
            const contentPage = await agent.Parse.getHtml(contentId);
            runInAction(() => {
                this.currentPageHtml = contentPage;
                this.currentPageContent = content;
                this.htmlLoaded = true;
                if (store.userStore.selectedProfile?.language !== content.language) {
                    store.userStore.setSelectedLanguage(content.language);
                }
            })
        } catch (error) {
           runInAction(() => this.htmlLoaded = true);
           console.log(error); 
        }
    }

    refreshTerm = (newTerm: UserTermDetails) => {
       for(let element of this.currentElementsMap.values()) {
            for (let t of element!.abstractTerms) {
                if (newTerm.termValue.toUpperCase() === t.termValue.toUpperCase()) {
                    t.rating = newTerm.rating;
                    t.starred = newTerm.starred;
                    t.easeFactor = newTerm.easeFactor;
                    t.timesSeen = newTerm.timesSeen;
                }
            }
       }
    }

    loadElementTerms = async (element: TextElement) => {
        try {
            const newTerms = await agent.Content.abstractTermsForElement({
                elementText: element.elementText,
                contentUrl: element.contentUrl,
                tag: element.tag,
                language: this.currentPageContent!.language
            });
            runInAction(() => {
                this.currentElementsMap.set(element.elementText, newTerms);
            })
        } catch (error) {
           console.log(error);
        }
    }
}