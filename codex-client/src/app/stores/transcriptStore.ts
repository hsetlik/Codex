import { makeAutoObservable, runInAction } from "mobx";
import { AbstractTerm } from "../models/userTerm";
import agent, { TermDto, UserTermCreateDto } from "../api/agent";

interface TranscriptChunkId{
    chunkId: string;
    index: number;
}

export default class TranscriptStore {
    //observable vars
    //just for endpoint access
    contentId: string = "null";
    transcriptChunkIds: TranscriptChunkId[] = [];
    currentChunkIndex: number = 0;
    //actually need to observe
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
            
            this.currentChunkIndex = 0;
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
        try {
            console.log(`Loading terms for chunk ${this.currentChunkIndex}`);
            const terms = await agent.Transcript.getAbstractTermsForChunk({transcriptChunkId: id});
            runInAction(() =>{
                console.log("Terms are loaded");
                this.termsAreLoaded = true;
                this.currentAbstractTerms = terms;
            });
        } catch (error) {
            console.log("Terms not loaded");
            console.log(error);
        } 
    }

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
    setSelectedTerm = (newTerm: AbstractTerm) => {
        this.selectedTerm = newTerm;
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
            })
        } catch (error) {
            console.log(error);
        }
    }
}