import { makeAutoObservable, runInAction } from 'mobx';
import agent, { IChildTranslation, UserTermCreateDto } from '../api/agent';
import { User, UserFormValues } from '../models/user';
import { AbstractTerm, UserTermDetails } from '../models/userTerm';
import { store } from './store';

export default class UserStore{
    user: User | null = null;

    languageProfiles: string[] = [];

    selectedLanguage: string = "none"

    constructor() {
        makeAutoObservable(this);
    }

    get isLoggedIn() { return !!this.user; }

    login = async (creds: UserFormValues) => {
        try {
            console.log("Starting login");
            const user = await agent.Account.login(creds);
            console.log("User found: " + user.username);
            runInAction(() => {
                this.user = user;
                console.log("User set");
                console.log(`User has token ${user.token}`);
                store.commonStore.setToken(user.token);
            } );
            const profiles = await agent.Profile.getUserProfiles();
            console.log("PROFILES FOUND");
            console.log(profiles);
            runInAction(() => {
                this.languageProfiles = [];
                for(var i = 0; i < profiles.length; ++i)
                {
                    this.languageProfiles.push(profiles[i].language);
                }
                
                console.log(this.languageProfiles);
                console.log("Selected: " + this.selectedLanguage);
            });
            this.setSelectedLanguage(user.lastStudiedLanguage);
            //redirect user to home page on successful login
            
            console.log(user);
        } catch (error) {
            throw error;
        }
    }

    logout = () => {
        console.log("Logging out...");
        store.commonStore.setToken(null);
        window.localStorage.removeItem('jwt');
        this.user = null;
        this.languageProfiles = [];
        this.selectedLanguage = "none";
        if(window.localStorage.getItem('jwt') != null)
        {
            console.log("WARNING: TOKEN NOT DELETED!!");
        }
        
    }

    setSelectedLanguage = (iso: string) => {
        console.log("Setting selected language: " + iso);
        this.selectedLanguage = iso;
        store.contentStore.loadMetadata(iso); //.finally(() => store.contentStore.loadKnownWords());
    }

    selectDefaultLanguage = () => {
        if (this.languageProfiles.length < 1) {
            console.log("No profiles loaded!");
            return;
        }
        let lang = this.user?.lastStudiedLanguage!;
        if (lang === null || lang === undefined)
        {
            lang = this.languageProfiles[0];
        }
        this.setSelectedLanguage(lang);
    }

    getUser = async () => {
        try {
            const user = await agent.Account.current();
            const profiles = await agent.Profile.getUserProfiles();
            runInAction(()=> {
                this.languageProfiles = [];
                for(var i = 0; i < profiles.length; ++i)
                {
                    this.languageProfiles.push(profiles[i].language);
                }
                this.selectedLanguage = this.languageProfiles[0];
                console.log(this.languageProfiles);
            }); 
            runInAction(() => this.user = user);
        } catch(error) {
            console.log(error);
        }
    }

    register = async (creds: UserFormValues) => {
        try {
            const user = await agent.Account.register(creds);
            store.commonStore.setToken(user.token);
            runInAction(() => this.user = user);
            
            console.log(user);
        } catch (error) {
            throw error;
        }

    }

    createTerm = async (term: UserTermCreateDto) => {
        console.log("Creating term for: " + term.termValue);
        try {
            await agent.UserTermEndpoints.create(term);
            await this.refreshByValue(term.termValue);
        } catch (error) {
            console.log(error);
        }
    }

    updateUserTerm = async (userTerm: UserTermDetails) => {
        try {
            //console.log(`Term seen ${userTerm.timesSeen} times`);
           await agent.UserTermEndpoints.updateUserTerm(userTerm);
           await this.refreshByValue(userTerm.termValue);
        } catch (error) {
           console.log(error); 
        }
    }

    refreshByValue = async (termValue: string) => {
        try {
            //console.log(`refreshing term with ID: ${termValue}`);
            let matchingTerms: Map<number, AbstractTerm> = new Map();
            for(var i = 0; i < store.contentStore.currentSectionTerms.abstractTerms.length; ++i)
            {
                const aTerm = store.contentStore.currentSectionTerms.abstractTerms[i];
                //console.log(`Checking term with value: ${aTerm.termValue} and index: ${i}`);
                if (aTerm.termValue.normalize() === termValue.normalize()) {
                    //console.log(`Found match with ${value} at Index ${i}`);
                    const newTerm = await agent.TermEndpoints.getAbstractTerm({value: aTerm.termValue, language: aTerm.language});
                    //console.log(`Reloaded Term: ${newTerm.termValue} at ${i}`);
                    newTerm.indexInChunk = i;
                    matchingTerms.set(i, newTerm);
                }
            }
            runInAction(() => {
                matchingTerms.forEach((value: AbstractTerm, key: number) => {
                    //update each term in the contentStore map
                    //console.log(`Term ${value} is at index ${key}`);
                    store.contentStore.currentSectionTerms.abstractTerms[key] = value;
                    if (key === store.contentStore.selectedTerm?.indexInChunk) {
                        store.contentStore.setSelectedTerm(value);
                    }
                });
                //make sure the selectedTerm observable gets updated if necessary
            })
        } catch (error) {
           console.log(error); 
        }
    }

    deleteTranslation = async (translation: IChildTranslation) => {
        try {
            await agent.UserTermEndpoints.deleteTranslation(translation);
            await store.contentStore.loadSelectedTermTranslations();
        } catch (error) {
           console.log(error); 
        }
    }

}