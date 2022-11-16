import type { IAssociation } from "@/domain/IAssociation";
import { BaseService } from "./BaseService";

export class AssociationService extends BaseService<IAssociation> {
  constructor() {
    super("association");
  }

}
