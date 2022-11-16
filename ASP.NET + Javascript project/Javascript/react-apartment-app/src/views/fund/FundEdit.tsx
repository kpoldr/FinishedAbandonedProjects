import { useEffect, useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { Link, useNavigate, useParams } from "react-router-dom";
import { IAssociation } from "../../domain/IAssociation";
import { IFund } from "../../domain/IFund";
import { AssociationService } from "../../services/AssociationService";
import { FundService } from "../../services/FundService";
import { useAppSelector } from "../../store/hooks";

const initialAssociationStateList: IAssociation[] = []

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
interface IFormInput {
    name: string;
    value: number;
    associationId: string;
}

function FundEdit() {
    let { id } = useParams();
    const associationService = new AssociationService();
    const fundService = new FundService();
    const jwt = useAppSelector((state) => state.identity);
    const navigate = useNavigate();

    const [associations, setAssociations] = useState(initialAssociationStateList);
    const [association, setAssociation] = useState(initialAssociationState);
    const [fund, setFund] = useState(initialFundState);

    useEffect(() => {
        console.log("did edit");
        if (id !== undefined) {
            fundService.get(id).then((data) => setFund(data));
            associationService.getAll().then((data) => setAssociations(data));
            console.log(association);
        }
        console.log(association);
    }, []);

    useEffect(() => {
        setValue("name", fund.name.en);
        setValue("value", fund.value);
        setValue("associationId", association.id!);
        associationService.get(fund.associationId).then((data) => setAssociation(data));
    }, [fund]);

    const {
        register,
        formState: { errors },
        handleSubmit,
        setValue,
    } = useForm<IFormInput>();

    const onSubmit: SubmitHandler<IFormInput> = (data) => TryEdit(data);

    const TryEdit = async (data: IFormInput) => {
        
        if (data.associationId === undefined){
            data.associationId = fund.associationId
        }

        if (id !== undefined) {
            var res = await fundService.update(id, {
                id: id,
                name: JSON.parse(`{"en": "${data.name}"}`),
                value: data.value,
                associationId: data.associationId
            });

            console.log(res);
            if (res.status >= 300 && res.errorMessage) {
                // let errorMessage = res.status + " " + res.errorMessage;
            } else {
                navigate("/Funds");
            }

            console.log(jwt);
        }
    };

    return (
        <>
            <h1>Edit</h1>

            <h4>Fund</h4>
            <hr />

            <div className="row">
                <form className="col-md-4" onSubmit={handleSubmit(onSubmit)}>
                <div>
                        <div className="form-group">
                            <label className="control-label">Name</label>
                            <input
                                {...register("name", {
                                    required: true,
                                })}
                                className="form-control"
                                type="text"
                            />
                            <div className="text-danger form-text">
                                {errors.name?.type === "required" &&
                                    "Name is required"}
                            </div>
                        </div>
                        <div className="form-group">
                            <label className="control-label">Value</label>
                            <input
                                {...register("value", {
                                    required: false,
                                })}
                                className="form-control"
                                type="number"
                            />
                        </div>

                        <div className="form-group">
                            <label className="control-label">Association Name</label>
                            <select
                                {...register("associationId", {
                                    required: false,
                                })}
                                className="form-control"
                            >   
                            {(association.name !== undefined) &&
                                <option selected value={association.id} hidden>{association.name.en}</option>}
                                {associations.map((item) => {
                                    return <option key={item.id} value={item.id}>{item.name.en}</option>;
                                })}
                            </select>
                            
                        </div>
                        <div className="form-group pt-1">
                            <input
                                type="submit"
                                value="Save"
                                className="btn btn-primary"
                            />
                            <Link
                                to={`/Funds`}
                                className="btn btn-primary m-1"
                            >
                                Back to List{" "}
                            </Link>{" "}
                        </div>
                    </div>
                </form>
            </div>
        </>
    );
}

export default FundEdit;
