import { LangStr } from "./LangStr";

export interface IAssociation {
    id?: string;
    name: LangStr
    description: LangStr | undefined;
    email: string;
    phone: number;
    bankName: LangStr | undefined;
    bankNumber: string | undefined;
    appUserId: string | undefined;
}

