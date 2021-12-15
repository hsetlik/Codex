import { AbstractTerm } from "./userTerm";

export interface TextElement {
    value: string,
    tag: string,
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

export interface ElementAbstractTerms {
    tag: string,
    abstractTerms: AbstractTerm
}

export interface SectionAbstractTerms {
    
}



