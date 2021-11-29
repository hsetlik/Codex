import { makeAutoObservable, runInAction } from "mobx";
import agent, { ContentHeaderDto, KnownWordsDto } from "../api/agent";
import { store } from "./store";


export default class ContentStore
{

    loadedHeaders: ContentHeaderDto[] = [];
    knownWordsLoaded = false;
    headerKnownWords: Map<string, KnownWordsDto> = new Map();

    selectedContentId: string = "none";
    constructor() {
        makeAutoObservable(this);
    }

    setSelectedContentId = (id: string) => {
        this.selectedContentId = id;
        store.transcriptStore.loadContent(id);
    }

    loadHeaders = async (lang: string) => {
        console.log("Loading headers...");
        try {
            var headers = await agent.Content.getLanguageContents({language: lang} );
            runInAction(() => this.loadedHeaders = headers); 
        } catch (error) {
          console.log("Content headers not loaded for: " + lang);
          console.log(error);  
        }
    }

    loadKnownWords = async () => {
        this.knownWordsLoaded = false;
        try {
            var map = new Map<string, KnownWordsDto>();
            for (const header of this.loadedHeaders) {
                var knownWords = await agent.Content.getKnownWordsForContent({contentId: header.contentId});
                map.set(header.contentId, knownWords);
            }
            runInAction(() => {
                this.headerKnownWords = map;
                this.knownWordsLoaded = true;
            })
        } catch (error) {
           console.log(error); 
           runInAction(() => this.knownWordsLoaded = true);
        }
    }
}