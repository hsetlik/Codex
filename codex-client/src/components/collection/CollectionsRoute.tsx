import { observer } from "mobx-react-lite";
import React from "react";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { useStore } from "../../app/stores/store";
import CollectionHeader from "./CollectionHeader";


export default observer(function CollectionsRoute() {
    const {lang} = useParams();
    const {collectionStore: {collectionsLoaded, currentCollections, currentCollectionsLanguage: currentCorrectionsLanguage, loadCollectionsForLanguage}} = useStore();
    useEffect(() => {
        if (lang !== currentCorrectionsLanguage || !collectionsLoaded) {
            loadCollectionsForLanguage(lang!);
        }
    }, [lang, collectionsLoaded, currentCorrectionsLanguage, loadCollectionsForLanguage])
    return (
        <div>
            {currentCollections.map(cln => (
                <CollectionHeader collection={cln} key={cln.collectionId} />
            ))}

        </div>
    )
})