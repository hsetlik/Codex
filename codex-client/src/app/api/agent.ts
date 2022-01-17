import axios, { AxiosError, AxiosResponse } from "axios";
import { store } from "../stores/store";
import { User, UserFormValues } from "../models/user";

import { toast } from "react-toastify";
import { AbstractTerm, Term, UserTerm, UserTermDetails } from "../models/userTerm";
import { UserTermCreateDto, AddTranslationDto,
         IUserTermId, TranslationDto, IChildTranslation, 
         TranslationResultDto, IContentId, 
         IContentUrl, ILanguageString, 
         KnownWordsDto, LanguageProfileDto, 
         SectionQueryDto, TermDto, 
         IProfileId, 
         ContentMsDto,
         ContentUrlDto,
         TranslatorQuery,
         SaveContentQuery,
         SavedContentDto,
         ICollectionId} from "../models/dtos";
import { ContentMetadata, ElementAbstractTerms, ContentSection, ContentTag, TagQuery, ContentPageHtml, ElementAbstractTermsQuery } from "../models/content";
import { MetricGraph, MetricGraphQuery } from "../models/dailyData";
import { Collection, CollectionsForLanguageQuery, CreateCollectionQuery } from "../models/collection";
import { AbstractPhrase, Phrase, PhraseCreateQuery, PhraseQuery } from "../models/phrase";

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
    return config;
  }
)

const responseBody = <T> (response: AxiosResponse<T>) => response.data;

//object to hold generic HTTP requests
const requests = {
    get:<T> (url: string) => axios.get<T>(url).then(responseBody),
    post:<T> (url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put:<T> (url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    del:<T> (url: string) => axios.delete<T>(url).then(responseBody)
}

const Account = {
    current: () => requests.get<User>('/Account'),
    login: (user: UserFormValues) => requests.post<User>('/Account/login', user),
    register: (user: UserFormValues) => requests.post<User>('/Account/register', user),
}

const Profile = {
    getUserProfiles: () => requests.get<LanguageProfileDto[]>('Profile/getUserProfiles'),
    allUserTerms: (lang: ILanguageString) => requests.post<UserTerm[]>('profile/allUserTerms', lang),
    getMetricGraph: (query: MetricGraphQuery) => requests.post<MetricGraph>('/profile/getMetricGraph', query),
    updateHistory: (id: IProfileId) => requests.post('profile/updateHistory', id)
}

const UserTermAgent = {
    create: (dto: UserTermCreateDto) => requests.post<UserTermCreateDto>('/userTerm/createUserTerm', dto),
    addTranslation: (dto: AddTranslationDto) => requests.post<AddTranslationDto>('/userTerm/addTranslation', dto),
    getUserTerm : (dto: Term) => requests.post<UserTerm>('/userTerm/getUserTerm', dto),
    updateUserTerm: (dto: UserTermDetails) => requests.post('/userTerm/updateUserTerm', dto),
    getTranslations: (dto: IUserTermId) => requests.post<TranslationDto[]>('/userTerm/getTranslations', dto),
    deleteTranslation: (translation: IChildTranslation) => requests.post('/userTerm/deleteTranslation', translation)
}

const TermEndpoints = {
     getAbstractTerm: (term: TermDto) => requests.post<AbstractTerm>('/term/getAbstractTerm', term)
}

//==============================================================================================================

const Content = {
    getLanguageContents: (language: ILanguageString) => requests.post<ContentMetadata[]>('/content/getLanguageContents', language),
    getKnownWordsForContent: (contentId: IContentId) => requests.post<KnownWordsDto>('/content/getKnownWordsForContent', contentId),
    abstractTermsForElement: (dto: ElementAbstractTermsQuery) => requests.post<ElementAbstractTerms>('/content/abstractTermsForElement', dto),
    getContentWithId: (contentId: IContentId) => requests.post<ContentMetadata>('/content/getContentWithId', contentId),
    viewContent: (dto: SectionQueryDto) => requests.post('/content/viewContent', dto),
    getBookmark: (contentUrl: IContentUrl) => requests.post<number>('/content/getBookmark', contentUrl),
    getSectionAtMs: (dto: ContentMsDto) => requests.post<ContentSection>('/content/getSectionAtMs', dto),
    saveContent: (dto: SaveContentQuery) => requests.post('/content/saveContent', dto),
    unsaveContent: (dto: SaveContentQuery) => requests.post('/content/unsaveContent', dto),
    getSavedContents: (id: IProfileId) => requests.post<SavedContentDto[]>('content/getSavedContents', id),
    addContentTag: (dto: ContentTag) => requests.post('content/addContentTag', dto),
    getContentsWithTag: (dto: TagQuery) => requests.post<ContentMetadata[]>('content/getContentsWithTag', dto),
    getContentPageHtml: (contentUrl: string) => requests.get<string>(contentUrl)
}

const CollectionAgent = {
    createCollection: (query: CreateCollectionQuery) => requests.post('collection/createCollection', query),
    deleteCollection: (query: ICollectionId) => requests.post('collection/deleteCollection', query),
    getCollection: (query: ICollectionId) => requests.post<Collection>('collection/getCollection', query),
    updateCollection: (dto: Collection) => requests.post('collection/updateCollection', dto),
    collectionsForLanguage: (query: CollectionsForLanguageQuery) => requests.post<Collection[]>('collection/collectionsForLanguage', query)
}

const Parse = {
    getSection: (dto: SectionQueryDto) => requests.post<ContentSection>('/parse/getSection', dto),
    getContentMetadata: (dto: ContentUrlDto) => requests.post<ContentMetadata>('/parse/getContentMetadata', dto),
    getHtml: (contentId: string) => requests.get<ContentPageHtml>(`/parse/getHtml/${contentId}`)
}

const Translate = {
    getTranslations: (dto: TermDto) => requests.post<TranslationResultDto[]>('translate/getTranslations', dto),
    getTranslation: (query: TranslatorQuery) => requests.post<TranslationDto>('translate/getTranslation', query)
}

const PhraseAgent = {
    createPhrase: (query: PhraseCreateQuery) => requests.post('phrase/createPhrase', query),
    getPhrase: (query: PhraseQuery) => requests.post<Phrase>('phrase/getPhraseDetails', query),
    getAbstractPhrase: (query: PhraseQuery) => requests.post<AbstractPhrase>('phrase/getAbstractPhrase', query),
    updatePhrase: (phrase: Phrase) => requests.post('phrase/updatePhrase', phrase)
}

//====================================================================================================================

const agent = {
    Account,
    Profile,
    Content,
    UserTermEndpoints: UserTermAgent,
    TermEndpoints,
    Parse,
    Translate,
    CollectionAgent,
    PhraseAgent
}

export default agent;