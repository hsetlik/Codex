import { makeAutoObservable } from "mobx";
import { Phrase, PhraseCreateQuery } from "../models/phrase";
import { AbstractTerm } from "../models/userTerm";



export default class PhraseStore {
    selectedPhrase: Phrase | null = null;
    currentSelectedTerms: AbstractTerm[] = [];
    constructor(){
        makeAutoObservable(this);
    }

    createPhrase = async (query: PhraseCreateQuery) => {
        
    }
}