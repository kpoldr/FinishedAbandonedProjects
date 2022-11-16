import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { IOwner } from "../../domain/IOwner";
import { OwnerService } from "../../services/OwnerService";

const initialState: IOwner = {
    id: "",
    name: { en: "" },
    email: "",
    phone: 0,
    advancedPay: 0,
    appUserId: "",
};


function OwnerDetails() {
    let { id } = useParams();
    const ownerService = new OwnerService();

    const [owner, setOwner] = useState(initialState);

    useEffect(() => {
        if (id !== undefined) {
            ownerService.get(id).then((data) => setOwner(data));
        }
    }, []);

    return (
        <>
            <h1>Details</h1>

            <div>
                <h4>Owner</h4>
                <hr />
                <dl className="row">
                    <dt className="col-sm-2">Name</dt>
                    <dd className="col-sm-10">{owner.name.en}</dd>
                    <dt className="col-sm-2">Email</dt>
                    <dd className="col-sm-10">{owner.email}</dd>
                    <dt className="col-sm-2">Phone</dt>
                    <dd className="col-sm-10">{owner.phone}</dd>
                    <dt className="col-sm-2">AdvancePay</dt>
                    <dd className="col-sm-10">{owner.advancedPay}</dd>
                </dl>
            </div>
            <div>
                <Link
                    to={`/Owners/Edit/${id}`}
                    className="btn btn-primary"
                >
                    Edit{" "}
                </Link>{" "}
                <Link to={`/Owners/`} className="btn btn-primary">
                    Back to List{" "}
                </Link>{" "}
            </div>
        </>
    );
}

export default OwnerDetails;
