import { createContext, useContext } from "react";
import CommonStore from "./commonStore";


interface Store {
  commonStore: CommonStore
}
export const store: Store = {
    commonStore: new CommonStore()
}

export const storeContext = createContext(store);

export function useStore() {
   return useContext(storeContext);
}