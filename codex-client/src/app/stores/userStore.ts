import { makeAutoObservable, runInAction } from 'mobx';
import agent from '../api/agent';
import { IChildTranslation, LanguageProfileDto, UserTermCreateDto } from '../models/dtos';
import { User, UserFormValues } from '../models/user';
import { UserTermDetails } from '../models/userTerm';
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
        store.contentStore.loadMetadata(iso).finally(() => store.contentStore.loadSavedContents(this.selectedProfile!.languageProfileId));
        if (store.knownWordsStore.knownWords.size > 0)
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
            await this.refreshByValue(term.termValue);
        } catch (error) {
            console.log(error);
        }
    }

    updateUserTerm = async (userTerm: UserTermDetails) => {
        store.htmlStore.refreshTerm(userTerm);
        try {
            await agent.UserTermEndpoints.updateUserTerm(userTerm);
            await this.refreshByValue(userTerm.termValue);
            console.log(`Term seen ${userTerm.timesSeen} times`);
        } catch (error) {
           console.log(error); 
        }
    }

    refreshByValue = async (termValue: string) => {
        try {
            let updatedTermValue = await agent.TermEndpoints.getAbstractTerm({value: termValue, language: this.selectedProfile?.language!});
            if (store.contentStore.selectedTerm?.termValue.toUpperCase() === termValue.toUpperCase()) {
                console.log(`Updating selected term with value ${termValue}`);
                let oldValue = store.contentStore.selectedTerm.termValue;
                updatedTermValue.termValue = oldValue;
                store.contentStore.selectTerm(updatedTermValue);
            }
            store.htmlStore.refreshTerm({...updatedTermValue});
            for(var i = 0; i < store.contentStore.currentSectionTerms.elementGroups.length; ++i)
            {
                for (var n = 0; n < store.contentStore.currentSectionTerms.elementGroups[i].abstractTerms.length; ++n)
                {
                    if (store.contentStore.currentSectionTerms.elementGroups[i].abstractTerms[n].termValue.toUpperCase() === termValue.toUpperCase()) {
                        let leading = store.contentStore.currentSectionTerms.elementGroups[i].abstractTerms[n].leadingCharacters;
                        let trailing = store.contentStore.currentSectionTerms.elementGroups[i].abstractTerms[n].trailingCharacters;
                        //let val = store.contentStore.currentSectionTerms.elementGroups[i].abstractTerms[n].termValue;
                        //updatedTermValue.termValue = val;
                        updatedTermValue.leadingCharacters = leading;
                        updatedTermValue.trailingCharacters = trailing;
                        updatedTermValue.indexInChunk = n; 
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