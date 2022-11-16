import type { IFund } from "@/domain/IFunds";
import { defineStore } from "pinia";

export const useFundStore = defineStore({
  id: "fund",
  state: () => ({ 
    funds: [] as IFund[],
  }),
  getters: {
    fundCount: (state) => state.funds.length,
  },
  actions: {
    Add(fund: IFund) {
      this.funds.push(fund);
    },
  },
});
