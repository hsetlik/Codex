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
    translations: string[]
}

export interface AbstractTerm {
    //common term props
    termValue: string,
    language: string,
    //determine which type of term
    hasUserTerm: boolean,
    //and the userTerm-specific props
    timesSeen: number,
    rating: number,
    easeFactor: number,
    translations: string[]
}

