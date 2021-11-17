import { makeAutoObservable, runInAction } from "mobx";
import agent, { ContentHeaderDto } from "../api/agent";
import { store } from "./store";


export default class ContentStore
{
    _loadedHeaders: ContentHeaderDto[] = [];

    selectedContentId: string = "none";
    constructor() {
        makeAutoObservable(this);
    }

    get loadedHeaders() {
        return this._loadedHeaders;
    }

    setSelectedContentId = (id: string) => {
        this.selectedContentId = id;
        store.transcriptStore.loadContent(id);
    }

    loadHeaders = async (lang: string) => {
        console.log("Loading headers...");
        try {
            var headers = await agent.Content.getLanguageContents({language: lang} );
            runInAction(() => this._loadedHeaders = headers); 
        } catch (error) {
          console.log("Content headers not loaded for: " + lang);
          console.log(error);  
        }
    }
}