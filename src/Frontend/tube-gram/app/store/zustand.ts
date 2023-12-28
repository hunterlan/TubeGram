import {create} from "zustand";
import {UserToken} from "@/app/models/users/user-token";

type UserStore = {
    userToken: UserToken | undefined
    add: (token: string, id: number) => void,
    remove: void
}

export const useUserStore = create<UserStore>((set) => ({
    userToken: undefined,
    add: (token: string, id: number) => set(() => ({userToken: new UserToken(id, token)})),
    remove: set(() => ({userToken: undefined}))
}));