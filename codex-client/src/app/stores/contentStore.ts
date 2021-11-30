import { makeAutoObservable, runInAction } from "mobx";
import agent, { ContentHeaderDto, KnownWordsDto } from "../api/agent";
import { store } from "./store";


export default class ContentStore
{

    headersLoaded = false;
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
        this.headersLoaded = false;
        this.loadedHeaders = [];
        console.log(`Loading Headers for: ${lang}`);
        try {
            var headers = await agent.Content.getLanguageContents({language: lang} );
            runInAction(() => {
                this.loadedHeaders = headers;
                this.headersLoaded = true;
            }); 
        } catch (error) {
          console.log(error);  
          runInAction(() => this.headersLoaded = true);
        }
        console.log(`Headers loaded for: ${lang}`);
    }

    loadKnownWords = async () => {
        this.knownWordsLoaded = false;
        this.headerKnownWords.clear();
        try {
            for (const header of this.loadedHeaders) {
                console.log(`loading words for header ${header.contentName}`);
                var knownWords = await agent.Content.getKnownWordsForContent({contentId: header.contentId});
                runInAction(() => {
                    this.headerKnownWords.set(header.contentId, knownWords);
                })
                console.log(`known words loaded for header ${header.contentName}`);
            }
            runInAction(() => {
                this.knownWordsLoaded = true;
            })
        } catch (error) {
           console.log(error); 
           runInAction(() => this.knownWordsLoaded = true);
        }
        
    }
}