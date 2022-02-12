import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { CaptionsQuery, VideoCaptionElement } from "../models/content";
import { store } from "./store";



const msInRangeGroup = (ms: number, captions: VideoCaptionElement[]): boolean => {
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
        // check to see if we need to switch yet
        var inRange = false;
        try {
            /*
            runInAction(() => {
                if (this.currentCaptions.length > 0) {
                    this.highlightedCaption = this.currentCaptions.find(c => c.startMs < ms && ms <= c.endMs) || null;
                    console.log(`highlighted caption is: ${this.highlightedCaption?.captionText}`);
                    let sectionStart = this.currentCaptions[0].startMs;
                    let sectionEnd = this.currentCaptions[this.currentCaptions.length - 1].endMs;
                    inRange = (this.currentCaptionsLoaded && ms >= sectionStart && ms < sectionEnd);
                    if(inRange) {
                        console.log(`In range for section ${sectionStart}-${sectionEnd}`);
                        return;
                    }
                }
            })
            */
            console.log(`Caption at ${ms} ms is out of range`);
                
            //Before any asynchronous API calls, try to
            runInAction(() => {
                // 1. make sure the correct caption is highlighted 
                
                //2. Check if we need to switch to buffer or load next section

                // 3. if the buffer is loaded and the playhead is in its range, we can use it and load the next buffer asynchronously
                if (this.bufferCaptionsLoaded && 
                    isBetween(ms, this.currentCaptions[this.currentCaptions.length - 1], this.bufferCaptions[0])) {
                    this.currentCaptions = this.bufferCaptions;
                    this.currentCaptionsLoaded = true;
                    this.bufferCaptions = [];
                    this.bufferCaptionsLoaded = false;
                    // since we switched to the buffer, we need to recalculate inRange
                    inRange = (this.currentCaptionsLoaded && msInRangeGroup(ms, this.currentCaptions));
                }
            });
        //only load the buffer
        if (!this.bufferCaptionsLoaded && this.currentCaptionsLoaded && inRange) {
            const query: CaptionsQuery = {
                videoId: store.termStore.selectedContent.videoId,
                language: store.userStore.selectedProfile?.language || 'null',
                fromMs: this.currentCaptions[this.currentCaptions.length - 1].endMs,
                numCaptions: 10
            }
            const newBufferElements = await agent.CaptionAgent.getCaptions(query);
            runInAction(() => {
                this.bufferCaptions = newBufferElements;
                this.bufferCaptionsLoaded = true;
            })
        }
        //if we don't have either buffer captions of current captions we load both
        else if (!this.bufferCaptionsLoaded && !this.currentCaptionsLoaded) {
            const query: CaptionsQuery = {
                videoId: store.termStore.selectedContent?.videoId || 'null',
                language: store.userStore.selectedProfile?.language || 'null',
                fromMs: ms,
                numCaptions: 20 // take 20 captions at once when loading both so we don't need two calls
            }
            const newCaptions = await agent.CaptionAgent.getCaptions(query);
            runInAction(() => {
                this.currentCaptions = newCaptions.slice(0, 9);
                this.bufferCaptions = newCaptions.slice(10, newCaptions.length - 1);
                this.currentCaptionsLoaded = true;
                this.bufferCaptionsLoaded = true;
            }) 
        }
    } catch (error) {
        console.log(error);
    }
    }

}