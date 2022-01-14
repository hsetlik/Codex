import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata, ContentPageHtml, ElementAbstractTerms } from "../models/content";
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
}