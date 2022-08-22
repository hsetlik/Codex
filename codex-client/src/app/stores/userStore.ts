import { makeAutoObservable, runInAction } from 'mobx';
import agent from '../api/agent';
import { IChildTranslation, LanguageProfileDto, UserTermCreateDto } from '../models/dtos';
import { User, UserFormValues } from '../models/user';
import { UserTerm } from '../models/userTerm';
import { store } from './store';

export default class UserStore{
    user: User | null = null;

    profilesLoaded = false;

    languageProfiles: LanguageProfileDto[] = [];

    selectedProfile: LanguageProfileDto | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    get isLoggedIn() { return !!this.user; }

    login = async (creds: UserFormValues) => {
        try {
            console.log(`Starting login for user ${creds.email}, ${creds.password}`);
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
            runInAction(() => {
                this.languageProfiles = profiles;
                this.profilesLoaded = true;
                const defaultProfile = this.languageProfiles.find(p => p.language === user.lastStudiedLanguage);
                const currentProfile = (defaultProfile !== undefined) ? defaultProfile : this.languageProfiles[0];
                this.selectedProfile = currentProfile;
                console.log(`Selected profile has id ${this.selectedProfile.languageProfileId} and language ${this.selectedProfile.language}}`);
            })
            console.log(profiles);
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
        if(window.localStorage.getItem('jwt') != null) {
            console.log("WARNING: TOKEN NOT DELETED!!");
        }
    }

    setSelectedLanguage = (iso: string) => {
        console.log("Setting selected language: " + iso);
        this.selectedProfile = this.languageProfiles.find(p => p.language === iso)!;
        if (store.knownWordsStore.difficulties.size > 0)
            store.knownWordsStore.clearKnownWords();
    }

    setSelectedProfile = (prof: LanguageProfileDto) => {
        this.selectedProfile = prof;
    }

    getUser = async () => {
        this.profilesLoaded = false;
        try {
            const user = await agent.Account.current();
            const profiles = await agent.Profile.getUserProfiles();
            runInAction(()=> {
                this.languageProfiles = profiles;
                this.profilesLoaded = true;
                this.user = user;
                const defaultProfile = this.languageProfiles.find(p => p.language === user.lastStudiedLanguage);
                const currentProfile = (defaultProfile !== undefined) ? defaultProfile : this.languageProfiles[0];
                this.selectedProfile = currentProfile;
                console.log(`Selected profile has id ${this.selectedProfile.languageProfileId} and language ${this.selectedProfile.language}}`);
            }); 
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
            //await this.refreshByValue(term.termValue);
            //runInAction(() => store.termStore.refreshAbstractTerm)
            const newTerm = await agent.UserTermEndpoints.getUserTerm({termValue: term.termValue, language: term.language});
            runInAction(() => {
                store.termStore.refreshTerm(newTerm);
            })
        } catch (error) {
            console.log(error);
        }
    }

    updateUserTerm = async (userTerm: UserTerm) => {
        try {
            await agent.UserTermEndpoints.updateUserTerm(userTerm);
            runInAction(()=> store.termStore.refreshTerm(userTerm));
        } catch (error) {
           console.log(error); 
        }
    }



    deleteTranslation = async (translation: IChildTranslation) => {
        try {
            await agent.UserTermEndpoints.deleteTranslation(translation);
        } catch (error) {
           console.log(error); 
        }
    }
}