export interface IJWTResponse {
    token: string,
    refreshToken: string;
    firstName: string;
    lastName: string;
    appUserId?: string;
}