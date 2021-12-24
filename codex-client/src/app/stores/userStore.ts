import { makeAutoObservable, runInAction } from 'mobx';
import agent from '../api/agent';
import { IChildTranslation, UserTermCreateDto } from '../models/dtos';
import { User, UserFormValues } from '../models/user';
import { UserTermDetails } from '../models/userTerm';
import { store } from './store';

export default class UserStore{
    user: User | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    get isLoggedIn() { return !!this.user; }

    login = async (creds: UserFormValues) => {
        try {
            console.log("Starting login");
            const user = await agent.Account.login(creds);
            console.log("User found: " + user.username);
            console.log("PROFILES FOUND");
            runInAction(() => {
                this.user = user;
                console.log("User set");
                console.log(`User has token ${user.token}`);
                store.commonStore.setToken(user.token);
            } );
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

    getUser = async () => {
        try {
            const user = await agent.Account.current();
            runInAction(()=> {
                this.user = user;
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
            let updatedTermValue = await agent.TermEndpoints.getAbstractTerm({value: termValue, language: 'TODO'});
            if (store.contentStore.selectedTerm?.termValue === termValue) {
                store.contentStore.setSelectedTerm(updatedTermValue);
            }
            for(var i = 0; i < store.contentStore.currentSectionTerms.elementGroups.length; ++i)
            {
                for (var n = 0; n < store.contentStore.currentSectionTerms.elementGroups[i].abstractTerms.length; ++n)
                {
                    if (store.contentStore.currentSectionTerms.elementGroups[i].abstractTerms[n].termValue === termValue) {
                        store.contentStore.setTermInSection(i, n, updatedTermValue);
                    }
                }
            }
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