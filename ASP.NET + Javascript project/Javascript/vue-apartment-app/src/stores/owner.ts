import type { IOwner } from "@/domain/IOwner";
import { defineStore } from "pinia";

export const useOwnerStore = defineStore({
  id: "owner",
  state: () => ({ 
    owners: [] as IOwner[],
  }),
  getters: {
    ownerCount: (state) => state.owners.length,
  },
  actions: {
    Add(owner: IOwner) {
      this.owners.push(owner);
    },
  },
});
