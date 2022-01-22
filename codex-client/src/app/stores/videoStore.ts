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
    currentTerms: Map<string, ElementAbstractTerms> = new Map();
    currentTermsLoaded = false;

    bufferCaptionsLoaded = false;
    bufferCaptions: VideoCaptionElement[] = [];
    bufferTerms: Map<string, ElementAbstractTerms> = new Map();
    bufferTermsLoaded = false;

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
                   fromMs: Math.round(ms),
                   numCaptions: 10
               })
               runInAction(() => {
                    this.currentCaptions = current;
                    this.currentCaptionsLoaded = true;
                    this.loadCurrentTermsAsync();
                });
            } catch (error) {
                console.log(error);
            }

            //load the buffer
           if (!this.bufferCaptionsLoaded && this.currentCaptionsLoaded) {
                try {
                const bufferStart = this.currentCaptions[this.currentCaptions.length - 1].endMs;
                const buffer = await agent.CaptionAgent.getCaptions({
                    videoId: store.contentStore.selectedContentMetadata?.videoId || 'null',
                    language: store.contentStore.selectedContentMetadata?.language || 'null',
                    fromMs: bufferStart,
                    numCaptions: 10
                });
                runInAction(() => { 
                    this.bufferCaptions = buffer;
                    this.bufferCaptionsLoaded = true;
                    console.log(`Buffer captions loaded for range ${buffer[0].startMs} to ${buffer[buffer.length - 1].endMs} ms`);
                    this.loadBufferTermsAsync(); 
                });
                } catch (error) {
                    console.log(error); 
                }
           }
        }
    }
    
    refreshTerm = (newTerm: UserTermDetails) => {
        console.log(`Refreshing all terms with`)
       for(let element of this.currentTerms.values()) {
            for (let t of element!.abstractTerms) {
                if (newTerm.termValue.toUpperCase() === t.termValue.toUpperCase()) {
                    t.hasUserTerm = true;
                    t.rating = newTerm.rating;
                    t.starred = newTerm.starred;
                    t.easeFactor = newTerm.easeFactor;
                    t.timesSeen = newTerm.timesSeen;
                    t.userTermId = newTerm.userTermId;
                }
            }
       }
    }

    loadCurrentTermsAsync = async () => {
        this. currentTermsLoaded = false;
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
            runInAction(() => this.currentTermsLoaded = true);

        } catch (error) {
           console.log(error); 
        }
    }
  
    loadBufferTermsAsync = async () => {
        this.bufferTerms.clear();
        this.bufferTermsLoaded = false;
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
            runInAction(() => this.bufferTermsLoaded = true);
        } catch (error) {
           console.log(error); 
        }
    }
}