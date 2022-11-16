import { LangStr } from "./LangStr";

export interface IOwner {
    id?: string;
    name: LangStr;
    email: string;
    phone: number;
    advancedPay: number;
    appUserId: string | undefined;
}