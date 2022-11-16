import type { IFund } from "../domain/IFund";
import { BaseService } from "./BaseService";

export class FundService extends BaseService<IFund> {
  constructor() {
    super("fund");
  }

}
