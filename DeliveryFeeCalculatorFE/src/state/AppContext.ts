import { createContext } from "react";

export interface IUserInfo {
    jwt: string;
    refreshToken: string;
    firstName: string;
    lastName: string;
    email: string;
    roles: string[];
}

export interface IuserContext {
    userInfo: IUserInfo | null | undefined;
    setUserInfo: (userInfo: IUserInfo | null | undefined) => void;
    hasLoadedFromStorage: boolean;
}

export const AppContext = createContext<IuserContext | null>(null);