import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Collection, CreateCollectionQuery } from "../models/collection";
import { ContentMetadata } from "../models/content";

export default class CollectionStore {

    collectionsLoaded = false;
    currentCollections: Map<string, Collection> = new Map();
    currentCollectionsLanguage = 'none';
    constructor() {
        makeAutoObservable(this);
    }

    loadCollectionsForLanguage = async (lang: string) => {
        this.collectionsLoaded = false;
        this.currentCollections.clear();
        try {
           const newCollections = await agent.CollectionAgent.collectionsForLanguage({language: lang, enforceVisibility: false});
           runInAction(() => {
               this.currentCollectionsLanguage = lang;
               this.collectionsLoaded = true;
               for(let col of newCollections) {
                   this.currentCollections.set(col.collectionId, col);
               }
           })
        } catch (error) {
            runInAction(() => this.collectionsLoaded = true);
            console.log(error); 
        }
    }

    addToCollection = async (collectionId: string, content: ContentMetadata) => {
        let coll = this.currentCollections.get(collectionId)!;
        coll.contents.push(content);
        try {
            await agent.CollectionAgent.updateCollection(coll);
        } catch (error) {
           console.log(error); 
        }
    }

    removeFromCollection = async (collectionId: string, content: ContentMetadata) => {
        const coll = this.currentCollections.get(collectionId)!;
        coll.contents = coll.contents.filter(c => c.contentId !== content.contentId);
        try {
            await agent.CollectionAgent.updateCollection(coll);
            runInAction(() => this.currentCollections.set(collectionId, coll))
        } catch (error) {
           console.log(error); 
        }
    }

    createCollection = async (query: CreateCollectionQuery) => {
        try {
            await agent.CollectionAgent.createCollection(query);
            runInAction(() => this.loadCollectionsForLanguage(this.currentCollectionsLanguage)) 
        } catch (error) {
            console.log(error); 
            runInAction(() => this.loadCollectionsForLanguage(this.currentCollectionsLanguage)) 
        }
    }
}