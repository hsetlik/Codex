export interface Term {
    termValue: string,
    language: string
}

export interface UserTerm {
    value: string,
    language: string,
    timesSeen: number,
    rating: number,
    easeFactor: number,
    translations: string[],
    userTermId: string
}

export interface UserTermDetails {
    termValue: string,
    language: string,
    timesSeen: number,
    rating: number,
    easeFactor: number,
    userTermId: string
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
    userTermId: string
}

export function AbstractToUserTermDetails(abstract: AbstractTerm) {
    const userTerm: UserTermDetails = {
        termValue: abstract.termValue,
        language: abstract.language,
        timesSeen: abstract.timesSeen,
        rating: abstract.rating,
        easeFactor: abstract.easeFactor,
        userTermId: abstract.userTermId
    }
    return userTerm;
}



