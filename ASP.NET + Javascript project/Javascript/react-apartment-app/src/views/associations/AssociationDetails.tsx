import { useEffect, useState } from "react";
import { Link,  useParams } from "react-router-dom";
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


function AssociationDetails() {
    let { id } = useParams();
    const associationService = new AssociationService();
    
    const [association, setAssociation] = useState(initialState);

    useEffect(() => {
        if (id != undefined) {
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
                <Link
                    to={`/Associations/Edit/${id}`}
                    className="btn btn-primary"
                >
                    Edit{" "}
                </Link>{" "}
                <Link to={`/Associations/`} className="btn btn-primary">
                    Back to List{" "}
                </Link>{" "}
            </div>
        </>
    );
}

export default AssociationDetails;
