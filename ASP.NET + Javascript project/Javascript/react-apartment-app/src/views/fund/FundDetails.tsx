import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { IAssociation } from "../../domain/IAssociation";
import { IFund } from "../../domain/IFund";
import { AssociationService } from "../../services/AssociationService";
import { FundService } from "../../services/FundService";

const initialFundState: IFund = {
    id: "",
    name: { en: "" },
    value: 0,
    associationId: "",
};

const initialAssociationState: IAssociation = {
    id: "",
    name: { en: "" },
    description: { en: "" },
    email: "",
    phone: 0,
    bankName: { en: "" },
    bankNumber: "",
    appUserId: "",
};

function FundDetails() {
    let { id } = useParams();
    const fundService = new FundService();
    const associationService = new AssociationService();

    const [association, setAssociation] = useState(initialAssociationState);
    const [fund, setFund] = useState(initialFundState);

    useEffect(() => {
        if (id !== undefined) {
            fundService.get(id).then((data) => setFund(data));
        }
    }, []);

    useEffect(() => {
        associationService
            .get(fund.associationId)
            .then((data) => setAssociation(data));
    }, [fund]);

    return (
        <>
            <h1>Details</h1>

            <div>
                <h4>Fund</h4>
                <hr />
                <dl className="row">
                    <dt className="col-sm-2">Name</dt>
                    <dd className="col-sm-10">{fund.name.en}</dd>
                    <dt className="col-sm-2">Value</dt>
                    <dd className="col-sm-10">{fund.value}</dd>

                    <dt className="col-sm-2">Association</dt>
                    {association.name !== undefined && (
                        <dd className="col-sm-10">{association.name.en}</dd>
                    )}
                </dl>
            </div>

            <div>
                <Link
                    to={`/Funds/Edit/${id}`}
                    className="btn btn-primary"
                >
                    Edit{" "}
                </Link>{" "}
                <Link to={`/Funds/`} className="btn btn-primary">
                    Back to List{" "}
                </Link>{" "}
            </div>
        </>
    );
}

export default FundDetails;
