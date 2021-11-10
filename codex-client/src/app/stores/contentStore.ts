import { makeAutoObservable, reaction, runInAction } from "mobx";
import agent, { ContentHeaderDto, ILanguageString } from "../api/agent";
import { ServerError } from "../models/serverError";

export default class ContentStore
{
    loadedHeaders: ContentHeaderDto[] = [];
    constructor() {
        makeAutoObservable(this);
    }

    loadHeaders = async (lang: ILanguageString) => {
        try {
            var headers = await agent.Content.getLanguageContents(lang);
            runInAction(() => this.loadedHeaders = headers); 
        } catch (error) {
          console.log(error);  
        }
    }




}