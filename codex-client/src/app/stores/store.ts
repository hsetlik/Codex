import { createContext, useContext } from "react";
import CommonStore from "./commonStore";
import UserStore from "./userStore";
import ModalStore from "./modalStore";
import ContentStore from "./contentStore";

interface Store {
  commonStore: CommonStore,
  userStore: UserStore,
  modalStore: ModalStore,
  contentStore: ContentStore,
}
export const store: Store = {
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore(),
    contentStore: new ContentStore(),
}

export const storeContext = createContext(store);

export function useStore() {
   return useContext(storeContext);
}