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
           }) 
        } catch (error) {
           console.log(error); 
           console.log(`loading content at: ${url} failed`);
           runInAction(() => {
               this.selectedContentUrl = url;
               this.selectedParagraphIndex = 0;
               this.selectedContentParagraphCount = 0;
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
                let newIndex = this.selectedParagraphIndex + 1;
                await this.loadParagraph(this.selectedContentUrl, newIndex);
                runInAction(() => this.selectedParagraphIndex = newIndex)
           } 
        } catch (error) {
           console.log(error); 
        }

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

    loadParagraph = async (url: string, pIndex: number) => {
        this.paragraphLoaded = false;
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
        this.selectedTerm = term;
    }
}