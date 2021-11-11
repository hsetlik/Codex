import { makeAutoObservable, runInAction } from "mobx";
import agent, { ContentHeaderDto, ILanguageString } from "../api/agent";


export default class ContentStore
{
    loadedHeaders: ContentHeaderDto[] = [];
    constructor() {
        makeAutoObservable(this);
    }

    loadHeaders = async (props: ILanguageString) => {
        try {
            var headers = await agent.Content.getLanguageContents(props);
            runInAction(() => this.loadedHeaders = headers); 
        } catch (error) {
          console.log(error);  
        }
    }
}