import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentPageHtml } from "../models/content";
import { store } from "./store";

export default class ArticleStore {
    htmlLoaded = false;
    currentPageHtml: ContentPageHtml | null = null;
    constructor() {
        makeAutoObservable(this);
    }

    loadPage = async (contentId: string) => {
        this.htmlLoaded = false;
        this.currentPageHtml = null;
        try {
            await store.termStore.selectContentByIdAsync(contentId);
            const content = store.termStore.selectedContent;
            const contentPage = await agent.Parse.getHtml(contentId);
            runInAction(() => {
                this.currentPageHtml = contentPage;
                this.htmlLoaded = true;
                if (store.userStore.selectedProfile?.language !== content.language) {
                    store.userStore.setSelectedLanguage(content.language);
                }
            })
            await agent.Content.viewContent({contentUrl: content.contentUrl, index: 0});
        } catch (error) {
           runInAction(() => this.htmlLoaded = true);
           console.log(error); 
        }
    }

}