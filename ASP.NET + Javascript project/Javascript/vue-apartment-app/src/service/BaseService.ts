import httpClient from "@/http-client";
import { useIdentityStore } from "@/stores/Identity";
import type { AxiosError } from "axios";
import { IdentityService } from "./IdentityService";
import type { IServiceResult } from "./IServiceResult";

export class BaseService<Tentity> {
  identityStore = useIdentityStore();

  constructor(private path: string) {}
  async getAll(): Promise<Tentity[]> {
    console.log("get all");
    try {
      let response = await httpClient.get(`/${this.path}`, {
        headers: {
          Authorization: "bearer " + this.identityStore.$state.jwt?.token,
        },
      });
      let res = response.data as Tentity[];
      return res;
    } catch (e) {
      let response = (e as AxiosError).response!;
      if (response.status == 401 && this.identityStore.jwt) {
        let identityService = new IdentityService();
        let refreshResponse = await identityService.refreshIdentity();
        this.identityStore.$state.jwt = refreshResponse.data!;

        if (!this.identityStore.$state.jwt) return [];

        let response = await httpClient.get(`/${this.path}`, {
          headers: {
            Authorization: "bearer " + this.identityStore.$state.jwt?.token,
          },
        });
        let res = response.data as Tentity[];
        return res;
      }
    }

    return [];
  }

  // async get(id: string): Promise<Tentity> {
  //   console.log("get");
  //   let response = await httpClient.get(`/${this.path}/${id}`);
  //   let res = response.data as Tentity;
  //   return res;
  // }

  async get(id: string): Promise<Tentity> {
    console.log("get");
    let response = await httpClient.get(`/${this.path}/${id}`, {
      headers: {
        Authorization: "bearer " + this.identityStore.$state.jwt?.token,
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
          Authorization: "bearer " + this.identityStore.$state.jwt?.token,
        },
      });
    } catch (e) {
      let res = {
        status: (e as AxiosError).response!.status,
        errorMessage: (e as AxiosError).response!.data.error,
      };

      console.log(res);
      return res;
    }

    return { status: response.status };
  }

  async update(id: string, entity: Tentity): Promise<IServiceResult<void>> {
    console.log("add");
    let response;
    try {
      response = await httpClient.put(`/${this.path}/${id}`, entity, {
        headers: {
          Authorization: "bearer " + this.identityStore.$state.jwt?.token,
        },
      });
    } catch (e) {
      let res = {
        status: (e as AxiosError).response!.status,
        errorMessage: (e as AxiosError).response!.data.error,
      };

      console.log(res);
      return res;
    }

    return { status: response.status };
  }

  async delete(id: string): Promise<IServiceResult<void>> {
    console.log("add");
    let response;
    try {
      response = await httpClient.delete(`/${this.path}/${id}`,{
        headers: {
          Authorization: "bearer " + this.identityStore.$state.jwt?.token,
        },
      });
    } catch (e) {
      let res = {
        status: (e as AxiosError).response!.status,
        errorMessage: (e as AxiosError).response!.data.error,
      };

      console.log(res);
      return res;
    }

    return { status: response.status };
  }
}
