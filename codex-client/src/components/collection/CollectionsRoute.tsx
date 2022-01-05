import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { getCollectionsArray } from "../../app/models/collection";
import { useStore } from "../../app/stores/store";
import CollectionHeader from "./CollectionHeader";


export default observer(function CollectionsRoute() {
    const {lang} = useParams();
    const {collectionStore: {collectionsLoaded, currentCollections, currentCollectionsLanguage, loadCollectionsForLanguage}} = useStore();
    useEffect(() => {
        if (lang !== currentCollectionsLanguage || !collectionsLoaded) {
            loadCollectionsForLanguage(lang!);
        }
    }, [lang, collectionsLoaded, currentCollectionsLanguage, loadCollectionsForLanguage]);
    const collectionsArray = getCollectionsArray(currentCollections);
    return (
        <div>
            { collectionsArray.map(col => (
                <CollectionHeader collection={col} key={col.collectionId} />
            ))
            }
        </div>
    )
})