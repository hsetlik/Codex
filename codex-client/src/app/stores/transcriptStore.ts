import { makeAutoObservable, runInAction } from "mobx";
import { AbstractTerm } from "../models/userTerm";
import { store } from "./store";
import agent from "../api/agent";

export default class TranscriptStore {
    //observable vars
    //just for endpoint access
    contentId: string = "null";
    transcriptChunkIds: string[] = [];
    currentChunkIndex: number = 0;
    //actually need to observe
    currentAbstractTerms: AbstractTerm[] = [];
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
            });
        } catch (error) {
            console.log(error);
        }
    }
    loadTermsForChunk = async (id: string) => {
        try {
            console.log("Loading terms. . . ");
            const terms = await agent.Transcript.getAbstractTermsForChunk({transcriptChunkId: id});
            runInAction(() => {
                this.termsAreLoaded = true;
                this.currentAbstractTerms = terms;
            });
        } catch (error) {
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
}