import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ElementAbstractTerms, VideoCaptionElement } from "../models/content";
import { UserTermDetails } from "../models/userTerm";
import { store } from "./store";



const msInRangeGroup = (ms: number, start: VideoCaptionElement, end: VideoCaptionElement): boolean => {
   return (ms > start.startMs && ms <= end.endMs);
}

export default class VideoStore {

    currentCaptionsLoaded = false;
    currentCaptions: VideoCaptionElement[] = [];
 
    bufferCaptionsLoaded = false;
    bufferCaptions: VideoCaptionElement[] = [];

    highlightedCaption: VideoCaptionElement | null = null;
    

    constructor(){
        makeAutoObservable(this);
    }

    loadForMs = async (ms: number) => {
        if (this.currentCaptions.length > 0)
            this.highlightedCaption = this.currentCaptions.find(c => c.startMs < ms && ms <= c.endMs) || null;
        if (this.currentCaptions.length > 0 && msInRangeGroup(ms, this.currentCaptions[0], this.currentCaptions[this.currentCaptions.length - 1])) {
            // nothing to do if ms is still in range of the current caption group
            console.log(`No update needed at ${ms} ms`);
            return;
        } else if (this.bufferCaptionsLoaded && msInRangeGroup(ms, this.bufferCaptions[0], this.bufferCaptions[this.bufferCaptions.length - 1])) {
            console.log(`Used buffer at ${ms} ms`);
            this.currentCaptions = this.bufferCaptions;
            this.currentCaptionsLoaded = true;

            this.bufferCaptions = [];
            this.bufferCaptionsLoaded = false;
        } else {
            // if neither the current nor the buffer are in range, we need to retreive both
            this.bufferCaptions = [];
            this.bufferCaptionsLoaded = false;

            this.currentCaptions = [];
            this.currentCaptionsLoaded = false;
            
            //load the current
            try {
               const current = await agent.CaptionAgent.getCaptions({
                    videoId: store.termStore.selectedContent.videoId,
                    language: store.termStore.selectedContent.language,
                    fromMs: Math.round(ms),
                    numCaptions: 10
               })
               runInAction(() => {
                    this.currentCaptions = current;
                    this.currentCaptionsLoaded = true;
                });
            } catch (error) {
                console.log(error);
            }

            //load the buffer
           if (!this.bufferCaptionsLoaded && this.currentCaptionsLoaded) {
                try {
                const bufferStart = this.currentCaptions[this.currentCaptions.length - 1].endMs;
                const buffer = await agent.CaptionAgent.getCaptions({
                    videoId: store.termStore.selectedContent.videoId,
                    language: store.termStore.selectedContent.language,
                    fromMs: bufferStart,
                    numCaptions: 10
                });
                runInAction(() => { 
                    this.bufferCaptions = buffer;
                    this.bufferCaptionsLoaded = true;
                    console.log(`Buffer captions loaded for range ${buffer[0].startMs} to ${buffer[buffer.length - 1].endMs} ms`);
                });
                } catch (error) {
                    console.log(error); 
                }
           }
        }
    }

}