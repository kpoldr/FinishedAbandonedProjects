import type { IJWTResponse } from "../domain/IJWTResponse";
import httpClient from "./HttpClient";
import { useAppSelector, useAppDispatch } from '../store/hooks'
import type { AxiosError } from "axios";
import type { IServiceResult } from "../domain/IServiceResult";
import { store } from "../store/store";


export class IdentityService {
    // jwt = useAppSelector(state => state.identity)
    // dispatch = useAppDispatch()
    // identityStore = useIdentityStore();
    
    async login(
        email: string,
        password: string
    ): Promise<IServiceResult<IJWTResponse>> {
        try {
            let loginInfo = {
                email,
                password,
            };
            let response = await httpClient.post(
                `/identity/account/login`,
                loginInfo
            );
            return {
                status: response.status,
                data: response.data as IJWTResponse,
            };
        } catch (e) {
            let errorMessage : any = (e as AxiosError).response

            let response = {
                status: (e as AxiosError).response!.status,
                errorMessage: errorMessage,
            };

            console.log(response);

            console.log((e as AxiosError).response);

            return response;
        }
    }

    async register(
        email: string,
        password: string,
        firstName: string,
        lastName: string
    ): Promise<IServiceResult<IJWTResponse>> {
        try {
            let loginInfo = {
                email,
                password,
                firstName,
                lastName,
            };

            let response = await httpClient.post(
                `/identity/account/register`,
                loginInfo
            );
            return {
                status: response.status,
                data: response.data as IJWTResponse,
            };
        } catch (e) {
            let errorMessage : any = (e as AxiosError).response!.data

            let response = {
                status: (e as AxiosError).response!.status,
                errorMessage: errorMessage.error,
            };

            console.log(response);

            console.log((e as AxiosError).response);

            return response;
        }
    }

    async refreshIdentity(): Promise<IServiceResult<IJWTResponse>> {
        try {
            let response = await httpClient.post(
                `/identity/account/refreshtoken`,
                {
                    jwt: store.getState().identity.token,
                    refreshToken: store.getState().identity.refreshToken
                }
            );
            return {
                status: response.status,
                data: response.data as IJWTResponse,
            };
        } catch (e) {
            let errorMessage : any = (e as AxiosError).response!.data

            let response = {
                status: (e as AxiosError).response!.status,
                errorMessage: errorMessage.error,
            };

            console.log(response);

            console.log((e as AxiosError).response);

            return response;
        }
    }
}
