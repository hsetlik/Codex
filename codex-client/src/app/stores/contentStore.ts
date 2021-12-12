import { makeAutoObservable, runInAction } from "mobx";
import agent, { AddTranslationDto, ContentMetadataDto, KnownWordsDto, TermsFromSection } from "../api/agent";
import { AbstractTerm } from "../models/userTerm";



export default class ContentStore
{

    headersLoaded = false;
    loadedContents: ContentMetadataDto[] = [];
    knownWordsLoaded = false;
    contentKnownWords: Map<string, KnownWordsDto> = new Map();
    selectedTerm: AbstractTerm | null = null;
    translationsLoaded = false;

    selectedContentMetadata: ContentMetadataDto | null = null;
    selectedContentUrl: string = "none";
    selectedContentBookmark = 0;
    selectedSectionIndex = 0;
    selectedContentSectionCount = 0;
    sectionLoaded = false;
    currentSectionTerms: TermsFromSection = {
        contentUrl: this.selectedContentUrl,
        index: this.selectedSectionIndex,
        abstractTerms: [],
        sectionHeader: ''
    }

    constructor() {
        makeAutoObservable(this);
    }

    setSelectedContent = async (url: string) => {
        console.log(`Selecting Content: ${url}`);
        try {
           let newSectionCount = await agent.Content.getSectionCount({contentUrl: url});
           let newBookmark = await agent.Content.getBookmark({contentUrl: url});
           runInAction(() => {
               this.selectedContentUrl = url;
               this.selectedSectionIndex = newBookmark;
               this.selectedContentSectionCount = newSectionCount;
               let newMetadata = this.loadedContents.find(v => v.contentUrl === url);
               if (newMetadata !== undefined)
                this.selectedContentMetadata = newMetadata;
               console.log("First section loaded");
           }) 
        } catch (error) {
           console.log(error); 
           console.log(`loading content at: ${url} failed`);
           runInAction(() => {
               this.selectedContentUrl = url;
               this.selectedSectionIndex = 0;
               this.selectedContentSectionCount = 0;
           }) 
        }
    }

    prevSection = async () => {
        try {
           if (this.selectedSectionIndex  > 0) {
                let newIndex = this.selectedSectionIndex - 1;
                await this.loadSectionById(this.selectedContentMetadata?.contentId!, newIndex);
                runInAction(() => this.selectedSectionIndex = newIndex)
           } 
        } catch (error) {
           console.log(error); 
        }
    }
    
    nextSection = async () => {
        try {
           if (this.selectedSectionIndex + 1 < this.selectedContentSectionCount) {
                console.log(`Loading section number ${this.selectedSectionIndex + 1} from URL ${this.selectedContentUrl}`);
                let newIndex = this.selectedSectionIndex + 1;
                await this.loadSectionById(this.selectedContentMetadata?.contentId!, newIndex);
                runInAction(() => this.selectedSectionIndex = newIndex)
           } 
        } catch (error) {
           console.log(error); 
        }

    }

    setTermAtIndex = (index: number, term: AbstractTerm) => {
        this.currentSectionTerms.abstractTerms[index] = term;
    }

    loadMetadata = async (lang: string) => {
        this.headersLoaded = false;
        this.loadedContents = [];
        console.log(`Loading Headers for: ${lang}`);
        try {
            var headers = await agent.Content.getLanguageContents({language: lang} );
            runInAction(() => {
                this.loadedContents = headers;
                this.headersLoaded = true;
            }); 
        } catch (error) {
          console.log(error);  
          runInAction(() => this.headersLoaded = true);
        }
        console.log(`Headers loaded for: ${lang}`);
    }

    /*
    loadKnownWords = async () => {
        this.knownWordsLoaded = false;
        this.contentKnownWords.clear();
        try {
            for (const header of this.loadedContents) {
                var knownWords = await agent.Content.getKnownWordsForContent({contentUrl: header.contentUrl});
                runInAction(() => {
                    this.contentKnownWords.set(header.contentUrl, knownWords);
                })
            }
            runInAction(() => {
                this.knownWordsLoaded = true;
            })
        } catch (error) {
           console.log(error); 
           runInAction(() => this.knownWordsLoaded = true);
        }
    }
    */

    addTranslation = async (dto: AddTranslationDto) => {
         this.translationsLoaded = false;
         try {
            await agent.UserTermEndpoints.addTranslation(dto);
            await this.loadSelectedTermTranslations();
            runInAction(() => this.translationsLoaded = true);
         } catch (error) {
            console.log("error");
            runInAction(() => this.translationsLoaded = true);
         }
     }

    loadSectionById = async (id: string, pIndex: number) => {
        this.sectionLoaded = false;
        try {
           let content = await agent.Content.getContentWithId({contentId: id}); 
           let newSection = await agent.Content.abstractTermsForSection({contentUrl: content.contentUrl, index: pIndex});
           let numSections = await agent.Content.getSectionCount({contentUrl: content.contentUrl});
           if (pIndex > 0) {
             await agent.Content.viewContent({contentUrl: content.contentUrl, index: pIndex});
           }
           runInAction(() => {
               this.sectionLoaded = true;
               this.currentSectionTerms = newSection;
               this.selectedContentSectionCount = numSections;
           })
        } catch (error) {
           console.log(error); 
           runInAction(() => this.sectionLoaded = true); 
        }
    }

    setSelectedTerm = (term: AbstractTerm) => {
        this.translationsLoaded = false;
        console.log(`Selecting term ${term.termValue} with language ${term.language} and index ${term.indexInChunk}`);
        this.selectedTerm = term;
    }

    loadSelectedTermTranslations = async () => {
        try {
           const translations =  await agent.UserTermEndpoints.getTranslations({userTermId: this.selectedTerm?.userTermId!});
           runInAction(() => {
               this.selectedTerm!.translations = [];
            for(const t of translations) {
                this.selectedTerm?.translations.push(t.value);
            }
            this.translationsLoaded = true;
           });
        } catch (error) {
           console.log(error);
        }
    }
}