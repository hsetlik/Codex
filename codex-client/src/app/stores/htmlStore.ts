import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata } from "../models/content";

export default class HtmlStore {
    htmlLoaded = false;
    currentHtml = '';
    currentUrl = '';
    currentPageContent: ContentMetadata | null = null;
    constructor() {
        makeAutoObservable(this);
    }

    loadPage = async (contentId: string) => {
        this.htmlLoaded = false;
        try {
            const content = await agent.Content.getContentWithId({contentId: contentId});
            const pageString = await agent.Content.getContentPageHtml(content.contentUrl);
            runInAction(() => {
                this.htmlLoaded = true;
                this.currentHtml = pageString;
                this.currentUrl = content.contentUrl;
                this.currentPageContent = content;
            })
        } catch (error) {
           runInAction(() => this.htmlLoaded = true);
           console.log(error); 
        }

    }
}