import { makeAutoObservable, runInAction } from "mobx";
import { Embed } from "semantic-ui-react";
import agent from "../api/agent";
import { ElementAbstractTerms, VideoCaptionElement } from "../models/content";
import { store } from "./store";



const msInRangeGroup = (ms: number, start: VideoCaptionElement, end: VideoCaptionElement): boolean => {
   return (ms > start.startMs && ms <= end.endMs);
}

export default class VideoStore {

    currentCaptionsLoaded = false;
    currentCaptions: VideoCaptionElement[] = [];
    currentTerms: Map<string, ElementAbstractTerms> = new Map();

    bufferCaptionsLoaded = false;
    bufferCaptions: VideoCaptionElement[] = [];
    bufferTerms: Map<string, ElementAbstractTerms> = new Map();
    

    constructor(){
        makeAutoObservable(this);
    }

    loadForMs = async (ms: number) => {
        if (msInRangeGroup(ms, this.currentCaptions[0], this.currentCaptions[this.currentCaptions.length - 1])) {
            // nothing to do if ms is still in range of the current caption group
            return;
        } else if (this.bufferCaptionsLoaded && msInRangeGroup(ms, this.bufferCaptions[0], this.bufferCaptions[this.bufferCaptions.length - 1])) {
            this.currentCaptions = this.bufferCaptions;
            this.currentTerms = this.bufferTerms;
            this.currentCaptionsLoaded = true;

            this.bufferCaptions = [];
            this.bufferTerms.clear();
            this.bufferCaptionsLoaded = false;
        } else {
            // if neither the current nor the buffer are in range, we need to retreive both
            this.bufferCaptions = [];
            this.bufferTerms.clear();
            this.bufferCaptionsLoaded = false;

            this.currentCaptions = [];
            this.currentTerms.clear();
            this.currentCaptionsLoaded = false;
            
            //load the current
            try {
               const current = await agent.CaptionAgent.getCaptions({
                   videoId: store.contentStore.selectedContentMetadata?.videoId || 'null',
                   language: store.contentStore.selectedContentMetadata?.language || 'null',
                   fromMs: ms,
                   numCaptions: 10
               })
               runInAction(() => this.currentCaptions = current);
               this.loadCurrentTermsAsync();
            } catch (error) {
                console.log(error);
            }

            //load the buffer
            try {
                const bufferStart = this.currentCaptions[this.currentCaptions.length - 1].endMs;
                const buffer = await agent.CaptionAgent.getCaptions({
                    videoId: store.contentStore.selectedContentMetadata?.videoId || 'null',
                    language: store.contentStore.selectedContentMetadata?.language || 'null',
                    fromMs: bufferStart,
                    numCaptions: 10
                });
                runInAction(() => this.bufferCaptions = buffer);
                this.loadBufferTermsAsync(); 
            } catch (error) {
               console.log(error); 
            }
        }
    }

    loadCurrentTermsAsync = async () => {
        this.currentTerms.clear();
        try {
            for(let cap of this.currentCaptions) {
                const terms = await agent.Content.abstractTermsForElement({
                    elementText: cap.captionText,
                    tag: 'caption',
                    contentUrl: store.contentStore.selectedContentMetadata?.contentUrl || 'null',
                    language: store.contentStore.selectedContentMetadata?.language || 'null'
                });
                runInAction(() => {
                    this.currentTerms.set(cap.captionText, terms);
                })
            }
            runInAction(() => this.currentCaptionsLoaded = true);

        } catch (error) {
           console.log(error); 
        }
    }
  
    loadBufferTermsAsync = async () => {
        this.bufferTerms.clear();
        try {
            for(let cap of this.bufferCaptions) {
                const terms = await agent.Content.abstractTermsForElement({
                    elementText: cap.captionText,
                    tag: 'caption',
                    contentUrl: store.contentStore.selectedContentMetadata?.contentUrl || 'null',
                    language: store.contentStore.selectedContentMetadata?.language || 'null'
                });
                runInAction(() => {
                    this.bufferTerms.set(cap.captionText, terms);
                })
            }
        } catch (error) {
           console.log(error); 
        }
    }
      
}