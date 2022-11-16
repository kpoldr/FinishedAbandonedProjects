import type { IFund } from "@/domain/IFunds";
import { BaseService } from "./BaseService";

export class FundService extends BaseService<IFund> {
  constructor() {
    super("fund");
  }

}
