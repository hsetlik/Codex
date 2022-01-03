import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { UserTerm } from "../models/userTerm";
import { store } from "./store";

export default class ProfileStore
{
    userTermsLoaded = false;
    currentUserTerms: UserTerm[] = [];
    currentLanguage: string = "";

    constructor(){
        makeAutoObservable(this);
    }

    loadProfile = async (lang: string) => {
        this.userTermsLoaded = false;
        try {
           const newUserTerms = await agent.Profile.allUserTerms({language: lang});
           runInAction(() => {
               this.userTermsLoaded = true;
               this.currentUserTerms = newUserTerms;
               this.currentLanguage = lang;
               store.userStore.setSelectedLanguage(lang);
           }) 
        } catch (error) {
           console.log(error);
           runInAction(() => {
            this.userTermsLoaded = true;
            this.currentUserTerms = [];
            this.currentLanguage = lang;
        })  
        }
    }
}