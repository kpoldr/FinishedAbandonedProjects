import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
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

function FundDelete() {
    let { id } = useParams();
    const fundService = new FundService();
    const associationService = new AssociationService();
    const navigate = useNavigate();


    const [association, setAssociation] = useState(initialAssociationState);
    const [fund, setFund] = useState(initialFundState);

    useEffect(() => {
        console.log("did fund get")
        if (id !== undefined) {
            fundService.get(id).then((data) => setFund(data));
        }
    }, []);

    useEffect(() => {
        console.log("did association get")
        associationService.get(fund.associationId).then((data) => setAssociation(data));
        
    }, [fund]);

    const TryDelete = async () => {
        let errorMessages = [];

        if (id !== undefined) {
            var res = await fundService.delete(id);

            if (res.status >= 300 && res.errorMessage) {
                errorMessages.push(res.status + " " + res.errorMessage);
            } else {
                navigate("/Funds");
            }
        }

        errorMessages.push("Id not found");
    };
    
    return (
        <>
            <h1>Delete</h1>

            <div>
                <h4>Are you sure you want to delete this fund?</h4>
                <hr />
                <dl className="row">
                    
                    <dt className="col-sm-2">Name</dt>
                    <dd className="col-sm-10">{fund.name.en}</dd>
                    <dt className="col-sm-2">Value</dt>
                    <dd className="col-sm-10">{fund.value}</dd>
                    
                    <dt className="col-sm-2">Association</dt>
                    {(association.name !== undefined) &&
                    <dd className="col-sm-10">{association.name.en}</dd>}
                </dl>
            </div>
            <div>
                <button className="btn btn-danger" onClick={TryDelete}>Delete </button>{" "}
                <Link to={`/Funds`} className="btn btn-primary">
                    Back to List{" "}
                </Link>{" "}
            </div>
        </>
    );
}

export default FundDelete;
