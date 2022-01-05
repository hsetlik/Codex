import { ContentMetadata } from "./content";

export interface Collection {
    collectionId: string,
    creatorLanguageProfileId: string,
    creatorUsername: string,
    isPrivate: boolean,
    createdAt: string,
    language: string,
    collectionName: string,
    description: string,
    contents: ContentMetadata[]
}

export interface CreateCollectionQuery {
    creatorUsername: string,
    isPrivate: boolean,
    language: string,
    collectionName: string,
    description: string,
    firstContentUrl: string
}

export interface CollectionsForLanguageQuery {
    language: string,
    enforceVisibility: boolean
}

export const getCollectionsArray = (map: Map<string, Collection>): Collection[] => {
    let out: Collection[] = [];
    map.forEach((value: Collection, key: string) => {
        out.push(value);
    })
    return out;
}