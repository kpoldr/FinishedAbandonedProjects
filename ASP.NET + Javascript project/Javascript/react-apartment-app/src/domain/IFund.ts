import { LangStr } from "./LangStr";

export interface IFund {
    id?: string;
    name: LangStr;
    value: number;
    associationId: string;
}