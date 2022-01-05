import { makeAutoObservable } from "mobx";
import agent from "../api/agent";
import { Collection } from "../models/collection";

export default class CollectionStore {

    collectionsLoaded = false;
    currentCollections: Collection[] = [];
    currentCorrectionsLanguage = 'none';
    constructor() {
        makeAutoObservable(this);
    }

    loadCollectionsForLanguage = async (lang: string) => {
        this.collectionsLoaded = false;
        try {
            
        } catch (error) {
           console.log(error); 
        }
    }

}