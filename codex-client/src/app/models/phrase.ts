

export interface PhraseCreateQuery {
    language: string,
    value: string,
    firstTranslation: string
}

export interface Phrase {
    phraseId: string,
    languageProfileId: string,
    language: string,
    value: string,
    translations: string[],
    timesSeen: number,
    easeFactor: number,
    srsIntervalDays: number,
    rating: number,
    createdAt: string
}

