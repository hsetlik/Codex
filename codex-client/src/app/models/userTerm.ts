export interface Term {
    termValue: string,
    language: string
}

export interface UserTerm {
    termValue: string,
    language: string,
    timesSeen: number,
    rating: number,
    easeFactor: number,
    translations: string[],
    userTermId: string,
    starred: boolean
}
export interface AbstractTerm {
    //common term props
    termValue: string,
    language: string,
    trailingCharacters: string,
    leadingCharacters: string,
    //determine which type of term
    hasUserTerm: boolean,
    //and the userTerm-specific props
    timesSeen: number,
    rating: number,
    easeFactor: number,
    translations: string[],
    indexInChunk: number
    userTermId: string,
    starred: boolean
}

export interface TermKey {
    parentElementText: string,
    indexInParent: number
}


