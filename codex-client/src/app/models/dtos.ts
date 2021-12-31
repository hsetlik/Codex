
export interface UserTermCreateDto {
    language: string,
    termValue: string,
    firstTranslation: string
}

export interface AddTranslationDto {
    userTermId: string,
    newTranslation: string
}
export interface TermDto {
    value: string,
    language: string
}

export interface TranslationResultDto {
    value: string,
    annotation: string
}

export interface IContentId {
    contentId: string
}

export interface IUserTermId {
    userTermId: string
}

export interface TranslationDto {
    termLanguage: string,
    termValue: string,
    userLanguage: string,
    userValue: string
}

export interface IChildTranslation {
    userTermId: string,
    value: string
}

export interface LanguageProfileDto {
    language: string,
    languageProfileId: string,
    knownWords: number
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

export interface IProfileId {
    languageProfileId: string
}

export interface KnownWordsDto {
    totalWords: number,
    knownWords: number
}

export interface SectionQueryDto {
    contentUrl: string,
    index: number
}

export interface ContentMsDto {
    contentUrl: string,
    ms: number
}
 export interface MillisecondsRange {
     start: number,
     end: number
 }


