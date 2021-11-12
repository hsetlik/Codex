import { makeAutoObservable } from "mobx";
import { AbstractTerm } from "../models/userTerm";
import { store } from "./store";
import agent from "../api/agent";

export default class TranscriptStore {
    //observable vars
    //just for endpoint access
    contentId: string = "null";
    transcriptId: string = "null";
    transcriptChunkIds: string[] = [];
    currentChunkIndex: number = 0;
    //actually need to observe
    currentAbstractTerms: AbstractTerm[] = [];
    constructor(){
        makeAutoObservable(this);
    }

    loadContent = async (id: string) => {
        this.contentId = id;
        
    }


}