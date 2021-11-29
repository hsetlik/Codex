import { makeAutoObservable, runInAction } from "mobx";
import { AbstractTerm } from "../models/userTerm";
import agent, { TermDto, AddTranslationDto, PopularTranslationDto } from "../api/agent";

interface TranscriptChunkId{
    chunkId: string;
    index: number;
}

export default class TranscriptStore {
    //observable vars
    //just for endpoint access
    contentId: string = "null";
    transcriptChunkIds: TranscriptChunkId[] = [];
    //actually need to observe
    popTranslationsLoaded = false;
    currentPopularTranslations: PopularTranslationDto[] = [];
    currentAbstractTerms: AbstractTerm[] = [];
    selectedTerm: AbstractTerm | null = null;
    termsAreLoaded: boolean = false;
    constructor(){
        makeAutoObservable(this);
    }

    loadContent = async (id: string) => {
        try {
            let tIds = await agent.Content.getChunksForContent({contentId: id});
            runInAction(() => {
            this.contentId = id;
            //make sure the transcriptChunkIds get assigned with correct indeces
            this.transcriptChunkIds = [];
            for (const id of tIds) {
               var dto: TranscriptChunkId = {
                chunkId: id.transcriptChunkId,
                index: id.transcriptChunkIndex
               };
               this.transcriptChunkIds.push(dto);
               console.log(`Added chunk number ${dto.index} with ID ${dto.chunkId}`);
            }
            });
            let firstChunkId = this.transcriptChunkIds.find(id => id.index === 0)!;
            let terms = await agent.Transcript.getAbstractTermsForChunk({transcriptChunkId: firstChunkId.chunkId})
            runInAction(() => {
            this.currentAbstractTerms = terms;
            this.termsAreLoaded = true;
            console.log(`Content for ${id} found`);
            });
            
        } catch (error) {
            console.log(error);
        }
    }

    loadTermsForChunk = async (id: string) => {
        this.termsAreLoaded = false;
        try {
            const terms = await agent.Transcript.getAbstractTermsForChunk({transcriptChunkId: id});
            runInAction(() =>{
                console.log("Terms are loaded");
                this.termsAreLoaded = true;
                this.currentAbstractTerms = terms;
                //now we need to figure out currentChunkIndex

            });
        } catch (error) {
            console.log("Terms not loaded");
            console.log(error);
            runInAction(() => this.termsAreLoaded = false);
        } 
    }

    chunkForIndex = (index: number) => {
        if (index < 0 || index >= this.transcriptChunkIds.length) {
            return null;
        }
        return this.transcriptChunkIds.find(t => t.index === index);
    }

  

    loadChunkAtIndex = async (index: number) => {
        this.termsAreLoaded = false;
        try {
           const newIndex = (index >= 0 && index < this.transcriptChunkIds.length) ? index : 0;
           const chunkId = this.transcriptChunkIds.find(t => t.index === index);
           if (chunkId) {
             await this.loadTermsForChunk(chunkId.chunkId);
           }
           runInAction(() => {
             this.termsAreLoaded = true;
           })
        } catch (error) {
           console.log(error)
           runInAction(() => {
            this.termsAreLoaded = true;
          }) 
        }
    }

    loadPopularTranslations = async () => {
        this.popTranslationsLoaded = false;
        this.currentPopularTranslations = [];
        try {
            const termDto: TermDto = {
                value: this.selectedTerm?.termValue!,
                language: this.selectedTerm?.language!
            }
            const translations = await agent.TermEndpoints.getPopularTranslations(termDto);
            runInAction(() => {
                this.currentPopularTranslations = translations;
                this.popTranslationsLoaded = true;
            })
        } catch (error) {
           console.log(error);
           runInAction(() => this.popTranslationsLoaded = true);
        }
    }

/*
    advanceChunk = async () => {
        try {
            var nextIndex = this.currentChunkIndex + 1;
            if (nextIndex >= this.transcriptChunkIds.length) {
                nextIndex = 0;
            }
            let id = this.transcriptChunkIds.find(id => id.index === nextIndex);
            runInAction(() => this.termsAreLoaded = false);
            if (id !== undefined) {
                this.loadTermsForChunk(id.chunkId);
            }
            runInAction(() => this.currentChunkIndex = nextIndex);  
        } catch (error) {
            console.log(error);
        }
        
    }

    previousChunk = async () => {
        try {
            var nextIndex = this.currentChunkIndex - 1;
            if (nextIndex < 0) {
                console.log("Already on chunk zero")
                return;
            }
            let id = this.transcriptChunkIds.find(id => id.index === nextIndex);
            runInAction(() => this.termsAreLoaded = false);
            if (id !== undefined) {
                await this.loadTermsForChunk(id.chunkId);
            }
            runInAction(() => this.currentChunkIndex = nextIndex);  
        } catch (error) {
            console.log(error);
        }
        
    }
*/
    setSelectedTerm = (newTerm: AbstractTerm) => {
        this.selectedTerm = newTerm;
        if (!this.selectedTerm.hasUserTerm) {
            this.loadPopularTranslations();
        }
        console.log("Selected term: " + this.selectedTerm.termValue);
    }

    refreshTerm = async (index: number) => {
        try {
            const term = this.currentAbstractTerms[index];
            const dto: TermDto = 
            {
                value: term.termValue,
                language: term.language
            };
            const newTerm = await agent.TermEndpoints.getAbstractTerm(dto);
            runInAction(() => {
                newTerm.indexInChunk = index;
                this.currentAbstractTerms[index] = newTerm;
                if (this.selectedTerm?.indexInChunk === index) {
                    this.selectedTerm = newTerm;
                }
            })
        } catch (error) {
            console.log(error);
        }
    }

    indecesWithValue = (value: string) => {
        var output: number[] = [];
        for (var term of this.currentAbstractTerms)
        {
            if (term.termValue.toUpperCase() === value.toUpperCase()) {
                output.push(term.indexInChunk);
            }
        }
        return output;
    }

    refershTermByValue = async (value: string) => {
        try {
            const toUpdate = this.indecesWithValue(value);
            for(const index of toUpdate) {
                await this.refreshTerm(index);
            }
           
        } catch (error) {
           console.log(error); 
        }
    }

    addTranslationToSelected = async (value: string) => {
        try {
            const userTermId = this.selectedTerm?.userTermId;
            const dto: AddTranslationDto = {
                userTermId: userTermId!,
                newTranslation: value
            };
            await agent.UserTermEndpoints.addTranslation(dto);
            runInAction(() => {
                this.refershTermByValue(this.selectedTerm?.termValue!);
            })
            
        } catch (error) {
            console.log(error);
        }
    }
}