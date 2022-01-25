import { AbstractTerm } from "./userTerm";



export interface TextElement {
    elementText: string,
    tag: string,
    contentUrl: string,
    index?: number,
    startMs?: number,
    endMs?: number
}

export interface VideoCaptionElement {
    captionText: string,
    startMs: number,
    endMs: number,
}

export interface CaptionsQuery {
    videoId: string,
    language: string,
    fromMs: number,
    numCaptions: number
}

export interface ElementAbstractTermsQuery {
    elementText: string,
    tag: string,
    contentUrl: string,
    language: string
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
    description?: string,
    creatorUsername?: string,
    languageProfileId?: string,
    videoId: string,
    audioUrl: string,
    contentType: string,
    contentName: string,
    dateAdded: string,
    language: string,
    bookmark: number,
    numSections: number,
    contentTags?: string[]
}

export interface ContentSectionMetadata {
    contentUrl: string,
    sectionHeader: string,
    index: number,
    numElements: number
}

export interface ElementAbstractTerms {
    elementText: string,
    tag?: string,
    abstractTerms: AbstractTerm[],
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

export interface ContentTag {
    contentId: string,
    tagValue: string,
    tagLanguage: string
}
export interface TagQuery {
    contentLanguage: string,
    tagValue: string,
    tagLanguage: string
}

export interface ContentPageHtml {
    contentUrl: string,
    html: string,
    stylesheetUrls: string[]
}


export const getContentType = (url: string): ContentType => {
    if (url.includes('wikipedia'))
        return ContentType.Wikipedia;
    else if (url.includes('youtube'))
        return ContentType.Youtube;
    else
        return ContentType.Article;
}

export interface ContentDifficulty {
    languageProfileId: string,
    contentId: string,
    updatedAt: string,
    totalWords: number,
    knownWords: number
}

export interface ContentDifficultyQuery {
    languageProfileId: string,
    contentId: string
}







