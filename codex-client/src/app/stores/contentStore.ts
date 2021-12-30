import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata, ContentSection, SectionAbstractTerms, TextElement } from "../models/content";
import { AddTranslationDto, KnownWordsDto } from "../models/dtos";
import { AbstractTerm } from "../models/userTerm";
import { store } from "./store";



export default class ContentStore
{

    headersLoaded = false;
    loadedContents: ContentMetadata[] = [];
    knownWordsLoaded = false;
    contentKnownWords: Map<string, KnownWordsDto> = new Map();
    selectedTerm: AbstractTerm | null = null;
    translationsLoaded = false;

    selectedContentMetadata: ContentMetadata | null = null;
    selectedContentUrl: string = "none";
    selectedSectionIndex = 0;
    sectionLoaded = false;
    currentSection: ContentSection | null = null;
    currentSectionTerms: SectionAbstractTerms = {
        contentUrl: 'none',
        index: 0,
        sectionHeader: 'none',
        elementGroups: []
    } 
    highlightedElement: TextElement | null = null; // for captions

    bufferSection: ContentSection | null = null;
    bufferSectionTerms: SectionAbstractTerms = {
        contentUrl: 'none',
        index: 0,
        sectionHeader: 'none',
        elementGroups: []
    }
    bufferLoaded = false;  

    constructor() {
        makeAutoObservable(this);
    }

    setHighlightedElement = (element: TextElement) => {
        this.highlightedElement = element;
    }

    elementAtSeconds = (seconds: number): TextElement => {
        for(let element of this.currentSection?.textElements!) {
            if (element.startSeconds <= seconds && element.endSeconds > seconds)
                return element;
        }
        return this.currentSection?.textElements[0]!;
    }

    //NOTE: this is only for updating metadata. Actual sections will not be loaded until loadSection runs
    setSelectedContent = async (url: string) => {
        console.log(`Selecting Content: ${url}`);
        try {
           runInAction(() => {
               this.selectedContentUrl = url;
               this.selectedSectionIndex = 0;
               let newMetadata = this.loadedContents.find(v => v.contentUrl === url);
               if (newMetadata !== undefined)
               {
                this.selectedContentMetadata = newMetadata;
                this.selectedSectionIndex = this.selectedContentMetadata.bookmark;
               }
               console.log("First section loaded");
               store.userStore.setSelectedLanguage(this.selectedContentMetadata?.language!);
           }) 
        } catch (error) {
           console.log(error); 
           console.log(`loading content at: ${url} failed`);
           runInAction(() => {
               this.selectedContentUrl = url;
               this.selectedSectionIndex = 0;
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
           if (this.selectedSectionIndex + 1 < this.selectedContentMetadata?.numSections!) {
                console.log(`Loading section number ${this.selectedSectionIndex + 1} from URL ${this.selectedContentUrl}`);
                let newIndex = this.selectedSectionIndex + 1;
                await this.loadSectionById(this.selectedContentMetadata?.contentId!, newIndex);
                runInAction(() => this.selectedSectionIndex = newIndex)
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

    loadSectionById = async (id: string, pIndex: number, useBuffer: boolean = true) => {
        this.sectionLoaded = false;
        //if the section we need is already in the buffer, just switch it over
        if (this.bufferLoaded && this.bufferSection?.index === pIndex) {
            this.currentSection = this.bufferSection;
            this.currentSectionTerms = this.bufferSectionTerms;
            this.sectionLoaded = true;
            this.bufferLoaded = false;
            this.bufferSection = null;
            this.bufferSectionTerms = {
                contentUrl: 'none',
                index: 0,
                sectionHeader: 'none',
                elementGroups: []
            }; 
        } else { // if we can't take from the buffer, call API for section at pIndex first
            this.bufferLoaded = false;
            try {
                let content = await agent.Content.getContentWithId({contentId: id}); 
                let section = await agent.Parse.getSection({contentUrl: content.contentUrl, index: pIndex});
                runInAction(() => {
                    this.selectedContentMetadata = content;
                    this.currentSection = section;
                    this.currentSectionTerms = {
                        contentUrl: content.contentUrl,
                        index: pIndex,
                        sectionHeader: section.sectionHeader,
                        elementGroups: []
                    };
                    this.sectionLoaded = true;
                    if (store.userStore.selectedProfile?.language !== content.language) {
                        store.userStore.setSelectedLanguage(content.language);
                    }
                })
                for(var element of this.currentSection?.textElements!) {
                    const elementTerms = await agent.Content.abstractTermsForElement(element);
                    runInAction(() => {
                        this.currentSectionTerms.elementGroups.push(elementTerms);
                    })
                }
                await agent.Content.viewContent({contentUrl: content.contentUrl, index: pIndex});
                // and load the buffer as applicable
                if (useBuffer && pIndex + 1 < content.numSections) {
                    await this.loadBufferSectionById(id, pIndex + 1);
                }
             } catch (error) {
                console.log(error); 
                runInAction(() => this.sectionLoaded = true); 
             }
        }
    }

    loadBufferSectionById = async (id: string, pIndex: number) => {
        this.bufferLoaded = false;
        try {
           let content = await agent.Content.getContentWithId({contentId: id}); 
           let section = await agent.Parse.getSection({contentUrl: content.contentUrl, index: pIndex});
           runInAction(() => {
               this.bufferSection = section;
               this.bufferSectionTerms = {
                   contentUrl: content.contentUrl,
                   index: pIndex,
                   sectionHeader: section.sectionHeader,
                   elementGroups: []
               };
               this.bufferLoaded = true;
           })
           for(var element of this.bufferSection?.textElements!) {
               const elementTerms = await agent.Content.abstractTermsForElement(element);
               runInAction(() => {
                   this.bufferSectionTerms.elementGroups.push(elementTerms);
               })
           }
        } catch (error) {
           console.log(error); 
           runInAction(() => this.bufferLoaded = true); 
        }
    }

    setTermInSection(elementIndex: number, termIndex: number, term: AbstractTerm) {
        term.indexInChunk = termIndex;
        this.currentSectionTerms.elementGroups[elementIndex].abstractTerms[termIndex] = term;
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