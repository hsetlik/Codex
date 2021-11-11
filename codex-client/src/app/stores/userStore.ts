import { makeAutoObservable, runInAction } from 'mobx';
import { appHistory } from '../..';
import agent from '../api/agent';
import { User, UserFormValues } from '../models/user';
import { store } from './store';

export default class UserStore{
    user: User | null = null;

    languageProfiles: string[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    setLanguageProfiles = (arr: string[]) => {
        this.languageProfiles = arr;
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
                this.languageProfiles = profiles
                console.log(this.languageProfiles);
            });
            //redirect user to home page on successful login
            appHistory.push('/feed');
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
        if(window.localStorage.getItem('jwt') != null)
        {
            console.log("WARNING: TOKEN NOT DELETED!!");
        }
        appHistory.push('/');
    }

    getUser = async () => {
        try {
            const user = await agent.Account.current();
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
            //redirect user to home page on successful login
            //appHistory.push('/activities');
            //store.modalStore.closeModal();
            console.log(user);
        } catch (error) {
            throw error;
        }

    }

}