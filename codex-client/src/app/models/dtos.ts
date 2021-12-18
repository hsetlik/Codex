import { AbstractTerm } from "./userTerm";

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

export interface LangProfileItem {
    language: string
}

export interface ILanguageString {
    language: string
}

export interface IContentName {
    contentName: string
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

export interface ElementQueryDto {
    contentUrl: string,
    sectionIndex: number,
    elementIndex: number
}
