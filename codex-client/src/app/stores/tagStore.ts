import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata, TagQuery } from "../models/content";
import { store } from "./store";

export default class TagStore {
    tagContentsLoaded = false;
    tagContents: ContentMetadata[] = [];
    currentTag: TagQuery | null = null;
    constructor() {
        makeAutoObservable(this);
    }

    loadTag = async (value: string) => {
        this.tagContentsLoaded = false;
        const newTag: TagQuery = {
            tagValue: value,
            tagLanguage: store.userStore.user?.nativeLanguage || 'en',
            contentLanguage: store.userStore.selectedProfile?.language || 'en'
        }
        try {
            const newContents = await agent.Content.getContentsWithTag(newTag);
            runInAction(() => {
                this.tagContentsLoaded = true;
                this.tagContents = newContents;
                this.currentTag = newTag
            })
        } catch (error) {
           console.log(error); 
           runInAction(() => this.tagContentsLoaded = true);
        }
    }
}