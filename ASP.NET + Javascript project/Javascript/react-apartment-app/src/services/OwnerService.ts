import type { IOwner } from "../domain/IOwner";
import { BaseService } from "./BaseService";

export class OwnerService extends BaseService<IOwner> {
  constructor() {
    super("owner");
  }

}
