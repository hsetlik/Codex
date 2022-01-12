import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata, ElementAbstractTerms } from "../models/content";
import { store } from "./store";

export default class HtmlStore {
    htmlLoaded = false;
    currentHtml = '';
    currentUrl = '';
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
            const pageString = await agent.Parse.getRawHtml(contentId);
            runInAction(() => {
                this.htmlLoaded = true;
                this.currentHtml = pageString;
                this.currentUrl = content.contentUrl;
                this.currentPageContent = content;
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