import { createContext, useContext } from "react";
import CommonStore from "./commonStore";
import UserStore from "./userStore";
import ModalStore from "./modalStore";
import ContentStore from "./contentStore";
import ProfileStore from "./profileStore";

interface Store {
  commonStore: CommonStore,
  userStore: UserStore,
  modalStore: ModalStore,
  contentStore: ContentStore,
  profileStore: ProfileStore
}
export const store: Store = {
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore(),
    contentStore: new ContentStore(),
    profileStore: new ProfileStore()
}

export const storeContext = createContext(store);

export function useStore() {
   return useContext(storeContext);
}