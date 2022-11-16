import type { IAssociation } from "@/domain/IAssociation";
import { defineStore } from "pinia";

export const useAssociationsStore = defineStore({
  id: "association",
  state: () => ({ 
    associations: [] as IAssociation[],
  }),
  getters: {
    associationCount: (state) => state.associations.length,
  },
  actions: {
    Add(association: IAssociation) {
      this.associations.push(association);
    },
  },
});
