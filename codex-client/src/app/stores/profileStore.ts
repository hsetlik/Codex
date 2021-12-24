import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { LanguageProfileDto } from "../models/dtos";
import { UserTerm } from "../models/userTerm";
import { store } from "./store";

export default class ProfileStore
{
    userTermsLoaded = false;
    currentUserTerms: UserTerm[] = [];
    currentLanguage: string = "";

    profilesLoaded = false;
    languageProfiles: LanguageProfileDto[] = [];

    selectedProfile: LanguageProfileDto | null = null;

    constructor(){
        makeAutoObservable(this);
    }

    loadProfileVocab = async (lang: string) => {
        this.userTermsLoaded = false;
        try {
           const newUserTerms = await agent.Profile.allUserTerms({language: lang});
           runInAction(() => {
               this.userTermsLoaded = true;
               this.currentUserTerms = newUserTerms;
               this.currentLanguage = lang;
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

    setSelectedLanguage = (iso: string) => {
        //TODO
        let match = this.languageProfiles.find(p => p.language === iso);
        if (match) {
            this.setSelectedProfile(match);
        } else {
            console.log(`no matching profile for language: ${iso}`);
        }
    }

    setSelectedProfile = (prof: LanguageProfileDto) => {
        this.selectedProfile = prof;
    }

    getProfiles = async () => {
        this.profilesLoaded = false;
        try {
            const profiles = await agent.Profile.getUserProfiles(); 
            runInAction(() => {
                this.languageProfiles = profiles;
                this.profilesLoaded = true;
            })
        } catch (error) {
           console.log(error); 
        }
    }
}