import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata, ContentSection, ContentTag, SectionAbstractTerms } from "../models/content";
import { AddTranslationDto, MillisecondsRange, SavedContentDto } from "../models/dtos";
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
            runInAction(() => {
                
                this.termTranslationsLoaded = true;
            });
         } catch (error) {
            console.log("error");
            runInAction(() => this.termTranslationsLoaded = true);
         }
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