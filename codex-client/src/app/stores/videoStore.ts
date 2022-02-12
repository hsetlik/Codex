import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { VideoCaptionElement } from "../models/content";
import { store } from "./store";



const msInRangeGroup = (ms: number, captions: VideoCaptionElement[]): boolean => {
    if (captions.length < 2)
        return false;
    let start = captions[0];
    let end = captions[captions.length - 1];
   return (ms > start.startMs && ms <= end.endMs);
}

const isBetween = (ms: number, first: VideoCaptionElement, second: VideoCaptionElement): boolean => {
    return (ms >= first.endMs && ms < second.endMs);
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

    reset = () => {
        this.currentCaptions = [];
        this.currentCaptionsLoaded = false;
        this.bufferCaptions = [];
        this.bufferCaptionsLoaded = false;
    }

    loadForMs = async (ms: number) => {
        if (this.currentCaptionsLoaded === true && msInRangeGroup(ms, this.currentCaptions)) {
            console.log(`Caption at ${ms} ms is in range`)
            return;
        }
        this.currentCaptionsLoaded = false;
        try {
            let vidId = store.termStore.selectedContent.videoId;
            let lang = store.termStore.selectedContent.language;
            console.log(`requesting captions for language: ${lang} and video ID : ${vidId}`);
            const newCaptions = await agent.CaptionAgent.getCaptions({fromMs: Math.round(ms), videoId: vidId, language: lang, numCaptions: 10});
            runInAction(() => {
                this.currentCaptions = newCaptions;
                this.currentCaptionsLoaded = true;
                console.log(`New captions loaded at ${ms}`);
            })
        } catch (error) {
           console.log(error);
           runInAction(() => this.currentCaptionsLoaded = true);
        }
    }

}