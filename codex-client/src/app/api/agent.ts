import axios, { AxiosError, AxiosResponse } from "axios";
import { store } from "../stores/store";
import { User, UserFormValues } from "../models/user";

import { toast } from "react-toastify";
import { AbstractTerm, Term, UserTerm, UserTermDetails } from "../models/userTerm";


axios.defaults.baseURL = 'https://localhost:5001/api';

axios.interceptors.response.use(response => {
    return response;
}, (error: AxiosError) => {
const {data, status, config} = error.response!;
switch (status)
{
    case 400:
        if (config.method === 'get' && data.errors.hasOwnProperty('id')){
          console.log("Not Found"); 
        }
        if (data.errors) {
            const modalStateErrors = [];
            for(const key in data.errors) {
                if(data.errors[key]) {
                    modalStateErrors.push(data.errors[key]);
                }
            }
            throw modalStateErrors.flat();
        } else {
            toast.error(data);
        }
        break;
    case 401:
        toast.error('Unauthorized');
        break;
    case 404:
        toast.error('NotFound');
        break;
    case 500:
        store.commonStore.setServerError(data);
        break;
}
});

axios.interceptors.request.use(config => {
    let token = window.localStorage.getItem('jwt');
    config.headers = Object.assign({
      Authorization: `Bearer ${token}`
    }, config.headers);
    //console.log(config.headers);
    return config;
  }
)

export interface ILanguageString {
    language: string
}
//
const responseBody = <T> (response: AxiosResponse<T>) => response.data;
//object to hold generic HTTP requests
const requests = {
    get:<T> (url: string) => axios.get<T>(url).then(responseBody),
    post:<T> (url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put:<T> (url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    del:<T> (url: string) => axios.delete<T>(url).then(responseBody)
}

interface LangProfileItem {
    language: string
}

//use create one of these for each endpoint group
const Account = {
    current: () => requests.get<User>('/Account'),
    login: (user: UserFormValues) => requests.post<User>('/Account/login', user),
    register: (user: UserFormValues) => requests.post<User>('/Account/register', user),
}

const Profile = {
    getUserProfiles: () => requests.get<LangProfileItem[]>('Profile/getUserProfiles'),
    allUserTerms: (lang: ILanguageString) => requests.post<UserTerm[]>('profile/allUserTerms', lang)
}

export interface UserTermCreateDto {
    language: string,
    termValue: string,
    firstTranslation: string
}

export interface AddTranslationDto {
    userTermId: string,
    newTranslation: string
}

export interface PopularTranslationDto {
    value: string,
    numInstances: number
}

export interface IContentId {
    contentId: string
}

export interface IUserTermId {
    userTermId: string
}

export interface TranslationDto {
    translationId: string,
    value: string
}

export interface IChildTranslation {
    userTermId: string,
    value: string
}

const UserTermEndpoints = {
    create: (dto: UserTermCreateDto) => requests.post<UserTermCreateDto>('/userTerm/createUserTerm', dto),
    addTranslation: (dto: AddTranslationDto) => requests.post<AddTranslationDto>('/userTerm/addTranslation', dto),
    getUserTerm : (dto: Term) => requests.post<UserTerm>('/userTerm/getUserTerm', dto),
    updateUserTerm: (dto: UserTermDetails) => requests.post('/userTerm/updateUserTerm', dto),
    getTranslations: (dto: IUserTermId) => requests.post<TranslationDto[]>('/userTerm/getTranslations', dto),
    deleteTranslation: (translation: IChildTranslation) => requests.post('/userTerm/deleteTranslation', translation)
}
interface ITerm {
    value: string,
    language: string
}



const TermEndpoints = {
     getAbstractTerm: (term: ITerm) => requests.post<AbstractTerm>('/term/getAbstractTerm', term),
     getPopularTranslations: ({value, language}: ITerm) => requests.post<PopularTranslationDto[]>('/term/popularTranslationsFor', {value, language})
}

//==============================================================================================================
export interface ContentMetadataDto {
    videoUrl: string,
    audioUrl: string,
    contentName: string,
    contentType: string,
    language: string,
    dateAdded: string,
    contentUrl: string,
    contentId: string,
    bookmark: number,
    numSections: number
}

export interface IContentUrl {
    contentUrl: string
}

export interface KnownWordsDto {
    totalWords: number;
    knownWords: number;
}

export interface SectionQueryDto {
    contentUrl: string,
    index: number
}

export interface TermsFromSection {
    contentUrl: string,
    index: number,
    abstractTerms: AbstractTerm[],
    sectionHeader: string
}

export interface IContentName {
    contentName: string
}

const Content = {
    getLanguageContents: (language: ILanguageString) => requests.post<ContentMetadataDto[]>('/content/getLanguageContents', language),
    getKnownWordsForContent: (contentId: IContentId) => requests.post<KnownWordsDto>('/content/getKnownWordsForContent', contentId),
    abstractTermsForSection: (dto: SectionQueryDto) => requests.post<TermsFromSection>('/content/abstractTermsForSection', dto),
    getContentWithName: (contentName: IContentName) => requests.post<ContentMetadataDto>('/content/getContentWithName', contentName),
    getContentWithId: (contentId: IContentId) => requests.post<ContentMetadataDto>('/content/getContentWithId', contentId),
    viewContent: (dto: SectionQueryDto) => requests.post('/content/viewContent', dto),
    getBookmark: (contentUrl: IContentUrl) => requests.post<number>('/content/getBookmark', contentUrl)
}
//====================================================================================================================
const agent = {
    Account,
    Profile,
    Content,
    UserTermEndpoints,
    TermEndpoints
}
export default agent;