import { makeAutoObservable, runInAction } from "mobx";
import agent, { ContentHeaderDto, ILanguageString } from "../api/agent";


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