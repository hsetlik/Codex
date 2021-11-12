import { makeAutoObservable, runInAction } from "mobx";
import agent, { ContentHeaderDto, ILanguageString } from "../api/agent";


export default class ContentStore
{
    loadedHeaders: ContentHeaderDto[] = [];

    selectedContentId: string = "none";
    constructor() {
        makeAutoObservable(this);
    }

    setSelectedContentId = (id: string) => {
        this.selectedContentId = id;
    }

    loadHeaders = async (lang: string) => {
        try {
            var headers = await agent.Content.getLanguageContents({language: lang} );
            runInAction(() => this.loadedHeaders = headers); 
        } catch (error) {
          console.log("Content headers not loaded for: " + lang);
          console.log(error);  
        }
    }
}