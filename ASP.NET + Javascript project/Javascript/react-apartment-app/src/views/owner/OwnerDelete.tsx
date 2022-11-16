import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
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

function OwnerDelete() {
    let { id } = useParams();
    const ownerService = new OwnerService();
    const navigate = useNavigate();

    const [owner, setOwner] = useState(initialState);

    const TryDelete = async () => {
        let errorMessages = [];

        if (id !== undefined) {
            var res = await ownerService.delete(id);

            if (res.status >= 300 && res.errorMessage) {
                errorMessages.push(res.status + " " + res.errorMessage);
            } else {
                navigate("/Owners");
            }
        }

        errorMessages.push("Id not found");
    };

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
                <button className="btn btn-danger" onClick={TryDelete}>Delete </button>{" "}
                <Link to={`/Associations/`} className="btn btn-primary">
                    Back to List{" "}
                </Link>{" "}
            </div>
        </>
    );
}

export default OwnerDelete;
