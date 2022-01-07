import { makeAutoObservable } from "mobx";
import { Phrase, PhraseCreateQuery } from "../models/phrase";
import { AbstractTerm } from "../models/userTerm";



export default class PhraseStore {
    currentPhrase: Phrase | null = null;
    currentSelectedTerms: AbstractTerm[] = [];
    phraseMode = false;
    constructor(){
        makeAutoObservable(this);
    }

    updatePhrase = (lastTerm: AbstractTerm, currentTerm: AbstractTerm) => {
        if (this.phraseMode && this.currentSelectedTerms[this.currentSelectedTerms.length - 1] === lastTerm) {
            this.currentSelectedTerms.push(currentTerm);
        } else {
            this.startPhraseMode(lastTerm, currentTerm);
        }
    }


    startPhraseMode = (term1: AbstractTerm, term2: AbstractTerm) => {
        console.log('starting phrase mode');
        this.currentSelectedTerms = [term1, term2];
        this.phraseMode = true;
    }

    exitPhraseMode = () => {
        this.currentSelectedTerms = [];
        this.currentPhrase = null;
        this.phraseMode = false;
        console.log('finished phrase mode');
    }



    createPhrase = async (query: PhraseCreateQuery) => {

    }
}