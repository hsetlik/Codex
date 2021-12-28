import { AbstractTerm } from "./userTerm";

export interface DotnetTimeSpan {
    days: number;
    hours: number;
    milliseconds: number;
    minutes: number;
    seconds: number;
    ticks: number
    totalDays: number;
    totalHours: number;
    totalMilliseconds: number;
    totalMinutes: number;
    totalSeconds: number;
}

export const defaultTimeSpan = (): DotnetTimeSpan => {
    return {
        days: 0,
        hours: 0,
    milliseconds: 0,
    minutes: 0,
    seconds: 0,
    ticks: 0,
    totalDays: 0,
    totalHours: 0,
    totalMilliseconds: 0,
    totalMinutes: 0,
    totalSeconds: 0
    };
}

export interface TextElement {
    value: string,
    tag: string,
    index: number,
    contentUrl: string,
    timeSpan: DotnetTimeSpan
}

export interface ContentSection {
    contentUrl: string,
    index: number,
    sectionHeader: string,
    body: string,
    textElements: TextElement[]
}

export interface ContentMetadata {
    contentId: string,
    contentUrl: string,
    videoUrl: string,
    audioUrl: string,
    contentType: string,
    contentName: string,
    dateAdded: string,
    language: string,
    bookmark: number,
    numSections: number
}

export interface ContentSectionMetadata {
    contentUrl: string,
    sectionHeader: string,
    index: number,
    numElements: number
}

export interface ElementAbstractTerms {
    index: number,
    tag: string,
    abstractTerms: AbstractTerm[]
}

export interface SectionAbstractTerms {
    contentUrl: string,
    index: number,
    sectionHeader: string,
    elementGroups: ElementAbstractTerms[]
}

export enum ContentType {
    Wikipedia,
    Article,
    Youtube
}

export const getContentType = (url: string): ContentType => {
    if (url.includes('wikipedia'))
        return ContentType.Wikipedia;
    else if (url.includes('youtube'))
        return ContentType.Youtube;
    else
        return ContentType.Article;
}


