import { ContentMetadata } from "./content";

export const FeedTypes = [
    "Newest",
    "RecentlyViewed",
    "MostViewed"
]

export interface FeedRow {
    feedType: string,
    contents: ContentMetadata[]
}

export interface Feed {
    languageProfileId: string,
    rows: FeedRow[]
}

export interface FeedQuery {
    languageProfileId: string
}

interface FeedTypeStringPair {
    value: string,
    display: string
}

export const FeedTypeNames: FeedTypeStringPair[] = [
    {value: 'Newest', display: 'Newest'},
    {value: 'RecentlyViewed', display: 'Recently Viewed'},
    {value: 'MostViewed', display: 'Most Viewed'}
]




