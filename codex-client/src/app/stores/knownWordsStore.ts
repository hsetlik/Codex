import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentDifficulty } from "../models/content";
import { KnownWordsDto } from "../models/dtos";
import { store } from "./store";


export default class KnownWordStore {
    difficulties: Map<string, ContentDifficulty> = new Map();
    constructor() {
        makeAutoObservable(this);
    }

    loadKnownWordsFor = async (contentId: string) => {
        try {
            var difficulty = await agent.Content.getContentDifficulty({
                contentId: contentId,
                languageProfileId: store.userStore.selectedProfile?.languageProfileId || 'null'
            })
            runInAction(() => {
                this.difficulties.set(contentId, difficulty);
            })
        } catch (error) {
           console.log(error);
        }
    }

    clearKnownWords = () => {
        this.difficulties.clear();
    }
}