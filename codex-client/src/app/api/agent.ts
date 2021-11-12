import axios, { AxiosError, AxiosResponse } from "axios";
import { store } from "../stores/store";
import { User, UserFormValues } from "../models/user";

import { toast } from "react-toastify";
import { AbstractTerm } from "../models/userTerm";

const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay);
    })
}

axios.defaults.baseURL = 'https://localhost:5001/api';

axios.interceptors.response.use(async response => {
    await sleep(1000);
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
    const token = store.commonStore.token;
    if(token) config.headers!.Authorization = `Bearer ${token}`;
    return config;
})
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
    getUserProfiles: () => requests.get<LangProfileItem[]>('Profile/getUserProfiles')
}

interface UserTermCreateDto {
    language: string,
    termValue: string,
    firstTranslation: string
}

interface AddTranslationDto {
    userTermId: string,
    newTranslation: string
}

const UserTermEndpoints = {
    create: (dto: UserTermCreateDto) => requests.post<UserTermCreateDto>('/userTerm/createUserTerm', dto),
    addTranslation: (dto: AddTranslationDto) => requests.post<AddTranslationDto>('/userTerm/addTranslation', dto)
}

//==============================================================================================================
export interface ILanguageString {
    language: string
}
export interface ContentHeaderDto {
    hasVideo: boolean,
    hasAudio: boolean,
    contentName: string,
    contentType: string,
    language: string,
    dateAdded: string,
    contentId: string
}

export interface IContentId {
    contentId: string
}

export interface TranscriptChunkDto {
    transcriptChunkId: string;
    language: string;
    chunkText: string;
}

const Content = {
    getLanguageContents: (language: ILanguageString) => requests.post<ContentHeaderDto[]>('/Content/getLanguageContents', language),
    getChunksForContent: (contentId: IContentId) => requests.post<TranscriptChunkDto[]>('/Content/getChunksForContent', contentId),
    getChunkIdsForContent: (contentId: IContentId) => requests.post<string[]>('/Content/getChunkIdsForContent', contentId)
}
//====================================================================================================================
export interface ITranscriptChunkId {
    transcriptChunkId: string
}

const Transcript = {
    getAbstractTermsForChunk: (dto: ITranscriptChunkId) => requests.post<AbstractTerm[]>('/Transcript/getAbstractTermsForChunk', dto)
}



const agent = {
    Account,
    Content,
    Transcript,
    UserTermEndpoints
}
export default agent;