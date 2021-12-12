import { makeAutoObservable, runInAction } from "mobx";
import agent, { KnownWordsDto } from "../api/agent";


export default class KnownWordStore {
    knownWords: Map<string, KnownWordsDto> = new Map();
    constructor() {
        makeAutoObservable(this);
    }

    loadKnownWordsFor = async (contentId: string) => {
        try {
            var knownWords = await agent.Content.getKnownWordsForContent({contentId: contentId});
            runInAction(() => {
                this.knownWords.set(contentId, knownWords);
            })
        } catch (error) {
           console.log(error);
        }
    }

    clearKnownWords = () => {
        this.knownWords.clear();
    }
}