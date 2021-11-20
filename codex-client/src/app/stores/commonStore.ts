import { makeAutoObservable, reaction } from "mobx";
import { ServerError } from "../models/serverError";

export default class CommonStore {
    error: ServerError | null = null;
    token: string | null = localStorage.getItem('jwt');
    appLoaded: boolean = false;

    constructor(){
        makeAutoObservable(this);
        reaction(
            () => this.token,
            token => {
                console.log("Checking token in common store...");
                if(token) {
                    window.localStorage.setItem('jwt', token);
                    console.log("token saved to local storage");
                } else {
                    console.log("token is null!");
                    window.localStorage.removeItem('jwt');
                }
            }
        )
    }

    setServerError = (error: ServerError) => {
        this.error = error;
    }

    setToken = (token: string | null) => {
        console.log("setting token...");
        this.token = token;
        if(token) {
            console.log(`Setting token to: ${token}`);
            window.localStorage.setItem('jwt', token);
            console.log(`Local token is ${window.localStorage.getItem('jwt')}`)
        }    
        else
            console.log("Token is null!");
    }

    setAppLoaded = () => {
        this.appLoaded = true;
    }
}