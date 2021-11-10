import { makeAutoObservable, runInAction } from 'mobx';
import { appHistory } from '../..';
import agent from '../api/agent';
import { User, UserFormValues } from '../models/user';
import { store } from './store';

export default class UserStore{
    user: User | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    get isLoggedIn() { return !!this.user; }

    login = async (creds: UserFormValues) => {
        try {
            const user = await agent.Account.login(creds);
            store.commonStore.setToken(user.token);
            runInAction(() => this.user = user);
            //redirect user to home page on successful login
            //appHistory.push('/activities');
            console.log(user);
        } catch (error) {
            throw error;
        }
    }

    logout = () => {
        store.commonStore.setToken(null);
        window.localStorage.removeItem('jwt');
        this.user = null;
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