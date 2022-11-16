import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { IAssociation } from "../../domain/IAssociation";
import { AssociationService } from "../../services/AssociationService";

const initialState: IAssociation = {
    id: "",
    name: { en: "" },
    description: { en: "" },
    email: "",
    phone: 0,
    bankName: { en: "" },
    bankNumber: "",
    appUserId: "",
};

function AssociationDelete() {
    let { id } = useParams();
    const associationService = new AssociationService();
    const navigate = useNavigate();

    const [association, setAssociation] = useState(initialState);

    const TryDelete = async () => {
        const associationService = new AssociationService();
        let errorMessages = [];

        if (id !== undefined) {
            var res = await associationService.delete(id);

            if (res.status >= 300 && res.errorMessage) {
                errorMessages.push(res.status + " " + res.errorMessage);
            } else {
                navigate("/Associations");
            }
        }

        errorMessages.push("Id not found");
    };

    useEffect(() => {
        if (id !== undefined) {
            associationService.get(id).then((data) => setAssociation(data));
        }
    }, []);

    return (
        <>
            <h1>Details</h1>

            <div>
                <h4>Association</h4>
                <hr />
                <dl className="row">
                    <dt className="col-sm-2">Name</dt>
                    <dd className="col-sm-10">{association.name.en}</dd>
                    <dt className="col-sm-2">Description</dt>
                    <dd className="col-sm-10">{association.description?.en}</dd>
                    <dt className="col-sm-2">Email</dt>
                    <dd className="col-sm-10">{association.email}</dd>
                    <dt className="col-sm-2">Phone</dt>
                    <dd className="col-sm-10">{association.phone}</dd>
                    <dt className="col-sm-2">Bank name</dt>
                    <dd className="col-sm-10">{association.bankName?.en}</dd>
                    <dt className="col-sm-2">Bank number</dt>
                    <dd className="col-sm-10">{association.bankNumber}</dd>
                </dl>
            </div>
            <div>
                <button className="btn btn-danger" onClick={TryDelete}>Delete </button>{" "}
                <Link to={`/Associations/`} className="btn btn-primary">
                    Back to List{" "}
                </Link>{" "}
            </div>
        </>
    );
}

export default AssociationDelete;
