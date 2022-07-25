import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ContentMetadata } from "../models/content";
import { Feed } from "../models/feed";


export default class FeedStore {
    feedLoaded = false;
    currentFeed: Feed | null = null;
    constructor(){
        makeAutoObservable(this);
    }

    get allContents() {
        let contents: ContentMetadata[] = [];
        for(let row of this.currentFeed!.rows) {
            for(let cont of row.contents)
                contents.push(cont);
        }
        return contents;
    }
    loadFeed =  async (profileId: string) => {
        this.feedLoaded = false;
        try {
           const newFeed = await agent.FeedAgent.getFeed({languageProfileId: profileId});
           runInAction(() => {
                this.currentFeed = newFeed;
                this.feedLoaded = true;
           }) 
        } catch (error) {
           console.log(error); 
        }
    }
}