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


