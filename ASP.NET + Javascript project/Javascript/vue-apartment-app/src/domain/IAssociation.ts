export interface IAssociation {
    id?: string;
    name: string;
    description: string | undefined;
    email: string;
    phone: number;
    bankName: string | undefined;
    bankNumber: string | undefined;
    appUserId: string | undefined;
}