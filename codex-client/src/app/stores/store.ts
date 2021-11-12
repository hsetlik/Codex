import { createContext, useContext } from "react";
import CommonStore from "./commonStore";
import UserStore from "./userStore";
import ModalStore from "./modalStore";
import ContentStore from "./contentStore";
import TranscriptStore from "./transcriptStore";

interface Store {
  commonStore: CommonStore,
  userStore: UserStore,
  modalStore: ModalStore,
  contentStore: ContentStore,
  transcriptStore: TranscriptStore
}
export const store: Store = {
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore(),
    contentStore: new ContentStore(),
    transcriptStore: new TranscriptStore()
}

export const storeContext = createContext(store);

export function useStore() {
   return useContext(storeContext);
}