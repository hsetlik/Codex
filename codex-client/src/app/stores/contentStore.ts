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

    loadHeaders = async (props: ILanguageString) => {
        try {
            var headers = await agent.Content.getLanguageContents(props);
            runInAction(() => this.loadedHeaders = headers); 
        } catch (error) {
          console.log(error);  
        }
    }
}