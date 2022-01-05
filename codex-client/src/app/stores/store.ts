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

interface Store {
  commonStore: CommonStore,
  userStore: UserStore,
  modalStore: ModalStore,
  contentStore: ContentStore,
  profileStore: ProfileStore,
  translationStore: TranslationStore,
  knownWordsStore: KnownWordStore,
  dailyDataStore: DailyDataStore,
  collectionStore: CollectionStore
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
    collectionStore: new CollectionStore()
}

export const storeContext = createContext(store);

export function useStore() {
   return useContext(storeContext);
}