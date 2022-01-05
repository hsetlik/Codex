import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Collection } from "../models/collection";
import { ContentMetadata } from "../models/content";

export default class CollectionStore {

    collectionsLoaded = false;
    currentCollections: Collection[] = [];
    currentCollectionsLanguage = 'none';
    constructor() {
        makeAutoObservable(this);
    }

    loadCollectionsForLanguage = async (lang: string) => {
        this.collectionsLoaded = false;
        try {
           const newCollections = await agent.CollectionAgent.collectionsForLanguage({language: lang, enforceVisibility: false});
           runInAction(() => {
               this.currentCollectionsLanguage = lang;
               this.currentCollections = newCollections;
               this.collectionsLoaded = true;
           })
        } catch (error) {
            runInAction(() => this.collectionsLoaded = true);
            console.log(error); 
        }
    }

    addToCollection = async (collectionId: string, content: ContentMetadata) => {
        let coll = this.currentCollections.find(c => c.collectionId === collectionId)!;
        coll.contents.push(content);
        try {
            await agent.CollectionAgent.updateCollection(coll);
        } catch (error) {
           console.log(error); 
        }
    }

}