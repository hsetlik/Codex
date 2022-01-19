import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Feed } from "../models/feed";


export default class FeedStore {
    feedLoaded = false;
    currentFeed: Feed | null = null;
    constructor(){
        makeAutoObservable(this);
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