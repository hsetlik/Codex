import { makeAutoObservable, runInAction } from 'mobx';
import agent from '../api/agent';
import { User, UserFormValues } from '../models/user';
import { store } from './store';

export default class UserStore{
    user: User | null = null;

    languageProfiles: string[] = [];

    selectedLanguage: string = "none"

    selectedContent: string = "none" // the ContentId GUID

    constructor() {
        makeAutoObservable(this);
    }

    get isLoggedIn() { return !!this.user; }

    login = async (creds: UserFormValues) => {
        try {
            console.log("Starting login");
            const user = await agent.Account.login(creds);
            console.log("User found: " + user.username);
            store.commonStore.setToken(user.token);
            runInAction(() => this.user = user);
            const profiles = await agent.Account.getUserProfiles();
            console.log("PROFILES FOUND");
            console.log(profiles);
            runInAction(() => {
                this.languageProfiles = [];
                for(var i = 0; i < profiles.length; ++i)
                {
                    this.languageProfiles.push(profiles[i].language);
                }
                this.selectedLanguage = this.languageProfiles[0];
                console.log(this.languageProfiles);
                console.log("Selected: " + this.selectedLanguage);
            });
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
        this.selectedContent = "none";
        if(window.localStorage.getItem('jwt') != null)
        {
            console.log("WARNING: TOKEN NOT DELETED!!");
        }
        
    }

    setSelectedLanguage = (iso: string) => {
        this.selectedLanguage = iso;
        store.contentStore.loadHeaders(iso);
    }

    setSelectedContent = (guid: string) => {
        this.selectedContent = guid;
        store.contentStore.setSelectedContentId(guid);
    }

    selectDefaultLanguage = () => {
        if (this.languageProfiles.length < 1) {
            console.log("No profiles loaded!");
            return;
        }
        this.setSelectedContent(this.languageProfiles[0]);
    }

    initStoreValues = async () => {
        try {
            const user = await agent.Account.current();
            runInAction(() => this.user = user);
            const profiles = await agent.Account.getUserProfiles();
            runInAction(()=> {
                this.languageProfiles = [];
                for(var i = 0; i < profiles.length; ++i)
                {
                    this.languageProfiles.push(profiles[i].language);
                }
                this.selectedLanguage = this.languageProfiles[0];
                console.log(this.languageProfiles);
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
            //redirect user to home page on successful login
            //store.modalStore.closeModal();
            console.log(user);
        } catch (error) {
            throw error;
        }

    }

}