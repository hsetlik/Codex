import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata, ContentSection, ContentTag, ElementAbstractTerms, SectionAbstractTerms, TextElement } from "../models/content";
import { AddTranslationDto, KnownWordsDto, MillisecondsRange, SavedContentDto } from "../models/dtos";
import { AbstractPhrase, PhraseCreateQuery } from "../models/phrase";
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
    currentAbstractPhrase: AbstractPhrase | null = null;
    
    //highlightedElement

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
        } else {
            if (this.phraseMode) { console.log('exiting phrase mode')}
            this.phraseMode = false;
            this.phraseTerms = [];
        }
        this.selectedTerm = term;
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
                    const elementTerms = await agent.Content.abstractTermsForElement({
                        elementText: element.elementText,
                        contentUrl: element.contentUrl,
                        tag: element.tag,
                        language: this.selectedContentMetadata!.language
                    });
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
               const elementTerms = await agent.Content.abstractTermsForElement({
                        elementText: element.elementText,
                        contentUrl: element.contentUrl,
                        tag: element.tag,
                        language: this.selectedContentMetadata!.language
                    });
               runInAction(() => {
                   this.bufferSectionTerms.elementGroups.push(elementTerms);
               })
           }
        } catch (error) {
           console.log(error); 
           runInAction(() => this.bufferLoaded = true); 
        }
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

    // Phrase related stuff

    createPhrase = async (query: PhraseCreateQuery) => {
        try {
            await agent.PhraseAgent.createPhrase(query);
            const newPhrase = await agent.PhraseAgent.getAbstractPhrase({value: query.value, language: query.language});
            runInAction(() => this.currentAbstractPhrase = newPhrase);
        } catch (error) {
            console.log(error);
        }
    }

    updateAbstractPhrase = async () => {
        let currentPhraseValue = '';
        for(let i = 0; i < this.phraseTerms.length; ++i) {
            const term = this.phraseTerms[i];
            if (i > 0 && term.leadingCharacters !== 'none')
                currentPhraseValue += term.leadingCharacters;
            currentPhraseValue += term.termValue;
            if (term.trailingCharacters !== 'none')
                currentPhraseValue += term.trailingCharacters;
            if (i !== this.phraseTerms.length - 1)
                currentPhraseValue += ' ';
        }
        console.log(`Updated Phrase vlaue is: ${currentPhraseValue}`);
        try {
            const newPhrase = await agent.PhraseAgent.getAbstractPhrase({value: currentPhraseValue, language: store.userStore.selectedProfile?.language || 'en'});
            runInAction(() => this.currentAbstractPhrase = newPhrase);
        } catch (error) {
           console.log(error);
        }
    }

    addContentTag = async (tag: ContentTag) => {
        console.log(`Adding Tag: ${tag.tagValue}, ${tag.tagLanguage}, ${tag.contentId}`);
        try {
           await agent.Content.addContentTag(tag);
           runInAction(() => {
               let existing = store.feedStore.allContents.find(c => c.contentId === tag.contentId);
               if (existing) {
                   if (!existing.contentTags){
                    existing.contentTags = [];
                   }
                existing.contentTags.push(tag.tagValue);
               }
           })
        } catch (error) {
           console.log(error); 
        }
    }
}