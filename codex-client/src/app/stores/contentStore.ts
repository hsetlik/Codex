import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata, ContentSection, ElementAbstractTerms, SectionAbstractTerms, TextElement } from "../models/content";
import { AddTranslationDto, KnownWordsDto, MillisecondsRange, SavedContentDto } from "../models/dtos";
import { AbstractTerm } from "../models/userTerm";
import { store } from "./store";

export const sectionMsRange = (section: ContentSection | null): MillisecondsRange => {
    if (section === null)
        return {
            start: 0,
            end: 0
        }
    const startMs = section.textElements[0].startMs;
    const endMs = section.textElements[section.textElements.length - 1].endMs;
    return {
        start: startMs!,
        end: endMs!
    }
}


export default class ContentStore
{
    //content headers
    headersLoaded = false;
    loadedContents: ContentMetadata[] = [];
    knownWordsLoaded = false;
    contentKnownWords: Map<string, KnownWordsDto> = new Map();
    selectedTerm: AbstractTerm | null = null;
    termTranslationsLoaded = false;

    // selected content
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
    
    //phrase mode stuff
    phraseMode = false;
    phraseTerms: AbstractTerm[] = [];
    
    //highlightedElement
    highlightedElement: TextElement | null = null; // for captions

    // buffer section
    bufferSection: ContentSection | null = null;
    bufferSectionTerms: SectionAbstractTerms = {
        contentUrl: 'none',
        index: 0,
        sectionHeader: 'none',
        elementGroups: []
    }
    bufferLoaded = false;  

    //saved content
    savedContents: SavedContentDto[] = [];
    savedContentsLoaded = false;

    constructor() {
        makeAutoObservable(this);
    }

    selectTerm = (term: AbstractTerm, shiftDown?: boolean) => {
        this.termTranslationsLoaded = false;
        if (shiftDown) {
            console.log('shift is down!');
        }
        if (shiftDown && this.selectedTerm !== null) {
            let elem = this.getParentElement(this.selectedTerm);
            if (elem === this.getParentElement(term)) {
                console.log('entered phrase mode');
                let startIndex = Math.min(this.selectedTerm.indexInChunk, term.indexInChunk);
                let endIndex = Math.max(this.selectedTerm.indexInChunk, term.indexInChunk);
                this.phraseMode = true;
                this.phraseTerms = elem.abstractTerms.slice(startIndex, endIndex);
            }
        } else {
            if (this.phraseMode) { console.log('exiting phrase mode')}
            this.phraseMode = false;
            this.phraseTerms = [];
        }
        this.selectedTerm = term;
    }

    setHighlightedElement = (element: TextElement) => {
        this.highlightedElement = element;
    }

    elementAtMs = (ms: number): TextElement => {
        for(let element of this.currentSection?.textElements!) {
            if (element.startMs <= ms && element.endMs > ms)
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

    contentIsSaved = (contentUrl: string): boolean => {
        return this.savedContents.some(c => c.contentUrl === contentUrl);
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

     toggleContentSaved = async (contentUrl: string) => {
         if (this.contentIsSaved(contentUrl)) {
            try {
                await agent.Content.unsaveContent({contentUrl: contentUrl, languageProfileId: store.userStore.selectedProfile?.languageProfileId!}); 
                runInAction(() => {
                    // just remove it from the store, no need to make another API call
                    this.savedContents = this.savedContents.filter(c => c.contentUrl !== contentUrl);
                })
            } catch (error) {
                console.log(error);    
            }
         } else {
             try {
                await agent.Content.saveContent({contentUrl: contentUrl, languageProfileId: store.userStore.selectedProfile?.languageProfileId!});
                await this.loadSavedContents(store.userStore.selectedProfile?.languageProfileId!);
                
            } catch (error) {
                console.log(error);    
            }

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

    loadSavedContents = async (languageProfileId: string) => {
        console.log(`Loading saved contents for profile: ${languageProfileId}`);
        this.savedContentsLoaded = false;
        this.savedContents = [];
        try {
           const contents = await agent.Content.getSavedContents({languageProfileId: languageProfileId});
           runInAction(() => {
               this.savedContents = contents;
               this.savedContentsLoaded = true;
           })
        } catch (error) {
           console.log(error); 
           runInAction(() => this.savedContentsLoaded = true);
        }
        console.log(`Saved Contents loaded `);
        console.log(this.savedContents);
    }

    addTranslation = async (dto: AddTranslationDto) => {
         this.termTranslationsLoaded = false;
         try {
            await agent.UserTermEndpoints.addTranslation(dto);
            await this.loadSelectedTermTranslations();
            runInAction(() => this.termTranslationsLoaded = true);
         } catch (error) {
            console.log("error");
            runInAction(() => this.termTranslationsLoaded = true);
         }
    }

    loadSectionById = async (id: string, pIndex: number, useBuffer: boolean = true) => {
        this.sectionLoaded = false;
        //if the section we need is already in the buffer, just switch it over
        if (this.bufferLoaded && this.bufferSection?.index === pIndex) {
            //load buffer to current
            this.currentSection = this.bufferSection;
            this.currentSectionTerms = this.bufferSectionTerms;
            // empty buffer
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
        console.log(`Loading buffer section ${pIndex} for content ${id}`);
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

    loadSectionForMs = async (ms: number, contentUrl: string, useBuffer: boolean = true) => {
        this.sectionLoaded = false;
        this.bufferLoaded = false;
        //Before making any new API calls, check if the buffer matches the needed timestamp
        const bufferRange = sectionMsRange(this.bufferSection!);
        if (ms >= bufferRange.start && ms < bufferRange.end) {
            runInAction(() => {
                this.currentSection = this.bufferSection;
                this.currentSectionTerms = this.bufferSectionTerms;
                this.sectionLoaded = true;
            })
        } else {
            console.log(`Loading section at ${ms / 1000} seconds`);
            try {
                // load the load the metadata & elements
               const newSection = await agent.Content.getSectionAtMs({ms: Math.round(ms), contentUrl: contentUrl});
               runInAction(() => {
                  
                   this.currentSection = newSection;
                   this.currentSectionTerms = {
                       contentUrl: contentUrl,
                       index: newSection.index,
                       sectionHeader: newSection.sectionHeader,
                       elementGroups: []
                   };
                   this.sectionLoaded = true;
              })
               // load the element abstract terms asynchronously
              for(let element of this.currentSection?.textElements!) {
                  const group = await agent.Content.abstractTermsForElement(element);
                  runInAction(() => this.currentSectionTerms.elementGroups.push(group));
              }
           } catch (error) {
               console.log(error)
           }
        }
        // load the buffer
        if (useBuffer) {
            try {
                let contentId = this.selectedContentMetadata!.contentId;
                let currentIndex = this.currentSection!.index;
                if (currentIndex < this.selectedContentMetadata!.numSections + 1) {
                    await this.loadBufferSectionById(contentId, currentIndex + 1);
                }
            } catch (error) {
                console.log(error);
            }
       }
    }

    setTermInSection(elementIndex: number, termIndex: number, term: AbstractTerm) {
        term.indexInChunk = termIndex;
        this.currentSectionTerms.elementGroups[elementIndex].abstractTerms[termIndex] = term;
    }

    getParentElement = (term: AbstractTerm): ElementAbstractTerms=> {
        return this.currentSectionTerms.elementGroups.find(g => g.abstractTerms.some(t => t === term))!;
    }

    loadSelectedTermTranslations = async () => {
        this.termTranslationsLoaded = false;
        this.selectedTerm!.translations = [];
        try {
           const translations =  await agent.UserTermEndpoints.getTranslations({userTermId: this.selectedTerm?.userTermId!});
           runInAction(() => {
            for(const t of translations) {
                this.selectedTerm?.translations.push(t.userValue);
            }
            this.termTranslationsLoaded = true;
           });
        } catch (error) {
           console.log(error);
        }
        console.log(`Translations loaded for: ${this.selectedTerm?.termValue}`);
    }
}