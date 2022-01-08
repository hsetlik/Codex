import { createContext, useContext } from "react";
import CommonStore from "./commonStore";
import UserStore from "./userStore";
import ModalStore from "./modalStore";
import ContentStore from "./contentStore";
import ProfileStore from "./profileStore";
import TranslationStore from "./translationStore";
import KnownWordStore from "./knownWordsStore";
import DailyDataStore from "./dailyDataStore";
import CollectionStore from "./collectionStore";
import PhraseStore from "./phraseStore";
import TagStore from "./tagStore";

interface Store {
  commonStore: CommonStore,
  userStore: UserStore,
  modalStore: ModalStore,
  contentStore: ContentStore,
  profileStore: ProfileStore,
  translationStore: TranslationStore,
  knownWordsStore: KnownWordStore,
  dailyDataStore: DailyDataStore,
  collectionStore: CollectionStore,
  phraseStore: PhraseStore,
  tagStore: TagStore
}
export const store: Store = {
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore(),
    contentStore: new ContentStore(),
    profileStore: new ProfileStore(),
    translationStore: new TranslationStore(),
    knownWordsStore: new KnownWordStore(),
    dailyDataStore: new DailyDataStore(),
    collectionStore: new CollectionStore(),
    phraseStore: new PhraseStore(),
    tagStore: new TagStore()
}

export const storeContext = createContext(store);

export function useStore() {
   return useContext(storeContext);
}