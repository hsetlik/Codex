import { makeAutoObservable, runInAction } from "mobx";
import { AbstractTerm } from "../models/userTerm";
import agent, { TermDto, UserTermCreateDto } from "../api/agent";

export default class TranscriptStore {
    //observable vars
    //just for endpoint access
    contentId: string = "null";
    transcriptChunkIds: string[] = [];
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
            let tIds = await agent.Content.getChunkIdsForContent({contentId: id});
            runInAction(() => {
            this.contentId = id;
            this.transcriptChunkIds = tIds;
            this.currentChunkIndex = 0;
            });
            let firstChunkId = this.transcriptChunkIds[0];
            let terms = await agent.Transcript.getAbstractTermsForChunk({transcriptChunkId: firstChunkId})
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
            console.log("Loading terms. . . ");
            const terms = await agent.Transcript.getAbstractTermsForChunk({transcriptChunkId: id}).finally(() => {
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
            let id = this.transcriptChunkIds[nextIndex];
            runInAction(() => this.termsAreLoaded = false);
            await this.loadTermsForChunk(id);
            runInAction(() => this.currentChunkIndex = nextIndex);  
        } catch (error) {
            console.log(error);
        }
        
    }

    setSelectedTerm = (newTerm: AbstractTerm) => {
        console.log("Selected term: " + newTerm.termValue);
        this.selectedTerm = newTerm;
    }

    updateUserTerm = async (dto: UserTermCreateDto) => {
        try {
            const termDto: TermDto = {
                value: dto.termValue,
                language: dto.language
            }
            const newTerm = await agent.TermEndpoints.get(termDto);
            runInAction(() => {
                console.log("found existing UserTerm...");
                var index = 0;
                for (var i = 0; i < this.currentAbstractTerms.length; ++i) {
                    if (this.currentAbstractTerms[i].termValue === dto.termValue &&
                        this.currentAbstractTerms[i].language === dto.language) {
                        //set a couple properties left empty by the API
                        newTerm.indexInChunk = i;
                        // newTerm.trailingCharacters = "";
                        // newTerm.hasUserTerm = true;
                        this.currentAbstractTerms[i] = newTerm;
                        return;
                }
                }
            })
        } catch (error) {
            console.log(error);
        }
    }
}