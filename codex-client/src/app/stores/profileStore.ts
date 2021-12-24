import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { allMetricNames, MetricGraph, MetricGraphQuery } from "../models/dailyData";
import { LanguageProfileDto } from "../models/dtos";
import { UserTerm } from "../models/userTerm";

export default class ProfileStore
{
    userTermsLoaded = false;
    currentUserTerms: UserTerm[] = [];
    currentLanguage: string = "";

    profilesLoaded = false;
    languageProfiles: LanguageProfileDto[] = [];

    selectedProfile: LanguageProfileDto | null = null;
    graphLoaded = false;

    currentGraph: MetricGraph | null = null;
    currentMetricName = allMetricNames[0];
    currentNumDays = 7;

    constructor(){
        makeAutoObservable(this);
    }    
    
    loadMetricGraph = async (query: MetricGraphQuery) => {
        console.log(query);
        this.graphLoaded = false;
        try {
            const newGraph = await agent.Profile.getMetricGraph(query);
            runInAction(() => {
                this.currentGraph = newGraph;
                this.graphLoaded = true;
            }) 
        } catch (error) {
            console.log(error);
        }
    }

    setCurrentMetricName = (name: string) => {
        this.currentMetricName = name;
    }

    setCurrentNumDays = (value: number) => {
        this.currentNumDays = value;
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
                console.log(`Loaded Profiles: ${profiles}`);
                this.profilesLoaded = true;
            })
        } catch (error) {
           console.log(error); 
        }
    }
}