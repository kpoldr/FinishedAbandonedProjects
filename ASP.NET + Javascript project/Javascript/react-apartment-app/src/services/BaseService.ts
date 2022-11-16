import httpClient from "./HttpClient";
import { useAppSelector, useAppDispatch } from '../store/hooks'
import { update } from '../store/identity'

import type { AxiosError } from "axios";
import { IdentityService } from "./IdentityService";
import type { IServiceResult } from "../domain/IServiceResult";
import { store } from "../store/store";

export class BaseService<Tentity> {
  
  // jwt = useAppSelector(state => state.identity)
  // dispatch = useAppDispatch()
  // identityStore = useIdentityStore();

  constructor(private path: string) {}
  async getAll(): Promise<Tentity[]> {
    console.log("get all");
    try {
      let response = await httpClient.get(`/${this.path}`, {
        headers: {
          Authorization: "bearer " + store.getState().identity.token,
        },
      });
      let res = response.data as Tentity[];
      return res;
    } catch (e) {
      let response = (e as AxiosError).response!;
      if (response.status == 401 && store.getState().identity) {
        let identityService = new IdentityService();
        let refreshResponse = await identityService.refreshIdentity();
        // might need to use dispatch but we'll see
        store.dispatch(update(refreshResponse.data!))
        
        if (store.getState().identity.appUserId === "") return [];

        let response = await httpClient.get(`/${this.path}`, {
          headers: {
            Authorization: "bearer " + store.getState().identity.token,
          },
        });
        let res = response.data as Tentity[];
        return res;
      }
    }

    return [];
  }

  async get(id: string): Promise<Tentity> {
    console.log("get");
    let response = await httpClient.get(`/${this.path}/${id}`, {
      headers: {
        Authorization: "bearer " + store.getState().identity.token,
      },
    });
    let res = response.data as Tentity;
    return res;
  }

  async add(entity: Tentity): Promise<IServiceResult<void>> {
    console.log("add");
    let response;
    try {
      response = await httpClient.post(`/${this.path}`, entity, {
        headers: {
          Authorization: "bearer " + store.getState().identity.token,
        },
      });
    } catch (e) {
      
      let errorMessage : any = (e as AxiosError).response!.data

      let response = {
          status: (e as AxiosError).response!.status,
          errorMessage: errorMessage.errors,
      };

      console.log(response);
      return response;
    }

    return { status: response.status };
  }

  async update(id: string, entity: Tentity): Promise<IServiceResult<void>> {
    console.log("add");
    let response;
    try {
      response = await httpClient.put(`/${this.path}/${id}`, entity, {
        headers: {
          Authorization: "bearer " + store.getState().identity.token,
        },
      });
    } catch (e) {
      let errorMessage : any = (e as AxiosError).response!.data

      let response = {
          status: (e as AxiosError).response!.status,
          errorMessage: errorMessage.error,
      };

      console.log(response);
      return response;
    }

    return { status: response.status };
  }

  async delete(id: string): Promise<IServiceResult<void>> {
    console.log("add");
    let response;
    try {
      response = await httpClient.delete(`/${this.path}/${id}`,{
        headers: {
          Authorization: "bearer " + store.getState().identity.token,
        },
      });
    } catch (e) {
      let errorMessage : any = (e as AxiosError).response!.data

      let response = {
          status: (e as AxiosError).response!.status,
          errorMessage: errorMessage.error,
      };

      console.log(response);
      return response;
    }

    return { status: response.status };
  }
}
