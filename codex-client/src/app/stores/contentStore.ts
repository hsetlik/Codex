import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata, ContentSection, SectionAbstractTerms } from "../models/content";
import { AddTranslationDto, KnownWordsDto } from "../models/dtos";
import { AbstractTerm } from "../models/userTerm";



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

    constructor() {
        makeAutoObservable(this);
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

    loadSectionById = async (id: string, pIndex: number) => {
        this.sectionLoaded = false;
        try {
           let content = await agent.Content.getContentWithId({contentId: id}); 
           let section = await agent.Parse.getSection({contentUrl: content.contentUrl, index: pIndex});
           runInAction(() => {
               this.currentSection = section;
               this.currentSectionTerms = {
                   contentUrl: content.contentUrl,
                   index: pIndex,
                   sectionHeader: section.sectionHeader,
                   elementGroups: []
               };
               this.sectionLoaded = true;
           })
           for(var element of this.currentSection?.textElements!) {
               const elementTerms = await agent.Content.abstractTermsForElement(element);
               runInAction(() => {
                   this.currentSectionTerms.elementGroups.push(elementTerms);
               })
           }
           await agent.Content.viewContent({contentUrl: content.contentUrl, index: pIndex});
        } catch (error) {
           console.log(error); 
           runInAction(() => this.sectionLoaded = true); 
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