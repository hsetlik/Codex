import { Agent } from "http";
import { makeAutoObservable, runInAction } from "mobx";
import agent, { AddTranslationDto, ContentMetadataDto, KnownWordsDto, TermsFromParagraph } from "../api/agent";
import { AbstractTerm } from "../models/userTerm";
import { asTermValue } from "../utilities/stringUtility";


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
    selectedParagraphIndex = 0;
    selectedContentParagraphCount = 0;
    paragraphLoaded = false;
    currentParagraphTerms: TermsFromParagraph = {
        contentUrl: this.selectedContentUrl,
        index: this.selectedParagraphIndex,
        abstractTerms: []
    }

    constructor() {
        makeAutoObservable(this);
    }

    setSelectedContent = async (url: string) => {
        try {
           let newParagraphCount = await agent.Content.getParagraphCount({contentUrl: url});
           let newParagraph = await agent.Content.abstractTermsForParagraph({contentUrl: url, index: 0});
           runInAction(() => {
               this.selectedContentUrl = url;
               this.selectedParagraphIndex = 0;
               this.selectedContentParagraphCount = newParagraphCount;
               let newMetadata = this.loadedContents.find(v => v.contentUrl === url);
               if (newMetadata !== undefined)
                this.selectedContentMetadata = newMetadata;
               this.currentParagraphTerms = newParagraph;
               console.log("First paragraph loaded");
               this.paragraphLoaded = true;
           }) 
        } catch (error) {
           console.log(error); 
           console.log(`loading content at: ${url} failed`);
           runInAction(() => {
               this.selectedContentUrl = url;
               this.selectedParagraphIndex = 0;
               this.selectedContentParagraphCount = 0;
               this.paragraphLoaded = true;
           }) 
        }
    }

    setSelectedContentById = async (_contentId: string) => {
        try {
            const matchingContent = await agent.Content.getContentWithId({contentId: _contentId});
            console.log(`selecting content with ID : ${_contentId}`);
            await this.setSelectedContent(matchingContent.contentUrl);
        } catch (error) {
           console.log(error); 
        }
    }

    prevParagraph = async () => {
        try {
           if (this.selectedParagraphIndex  > 0) {
                let newIndex = this.selectedParagraphIndex - 1;
                await this.loadParagraph(this.selectedContentUrl, newIndex);
                runInAction(() => this.selectedParagraphIndex = newIndex)
           } 
        } catch (error) {
           console.log(error); 
        }
    }
    
    nextParagraph = async () => {
        try {
           if (this.selectedParagraphIndex + 1 < this.selectedContentParagraphCount) {
                console.log(`Loading paragraph number ${this.selectedParagraphIndex + 1} from URL ${this.selectedContentUrl}`);
                let newIndex = this.selectedParagraphIndex + 1;
                await this.loadParagraph(this.selectedContentUrl, newIndex);
                runInAction(() => this.selectedParagraphIndex = newIndex)
           } 
        } catch (error) {
           console.log(error); 
        }

    }

    setTermAtIndex = (index: number, term: AbstractTerm) => {
        this.currentParagraphTerms.abstractTerms[index] = term;
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

    addTranslationToTerm = async (term: AbstractTerm, translation: string) => {
        this.translationsLoaded = false;
        try {
           let dto: AddTranslationDto = {
               userTermId: term.userTermId,
               newTranslation: translation
           };
           await agent.UserTermEndpoints.addTranslation(dto);

        } catch (error) {
           console.log(error); 
        }
    }
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

    loadParagraph = async (url: string, pIndex: number) => {
        this.paragraphLoaded = false;
        console.log(`Loading paragraph number ${pIndex} of URL ${url}`);
        try {
            let newParagraph = await agent.Content.abstractTermsForParagraph({contentUrl: url, index: pIndex});
            runInAction(() => {
            this.paragraphLoaded = true;
            this.currentParagraphTerms = newParagraph});
        } catch (error) {
           console.log(error); 
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