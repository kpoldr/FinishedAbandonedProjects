import { useEffect, useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { Link, useNavigate } from "react-router-dom";
import { IAssociation } from "../../domain/IAssociation";

import { AssociationService } from "../../services/AssociationService";
import { FundService } from "../../services/FundService";

const initialState: IAssociation[] = [];

interface IFormInput {
    name: string;
    value: number;
    associationId: string;
}

function FundCreate() {
    const associationService = new AssociationService();

    const navigate = useNavigate();
    const [associations, setAssociations] = useState(initialState);

    useEffect(() => {
        associationService.getAll().then((data) => setAssociations(data));
    }, []);

    const {
        register,
        formState: { errors },
        handleSubmit,
    } = useForm<IFormInput>();

    const onSubmit: SubmitHandler<IFormInput> = (data) => TryCreate(data);

    const TryCreate = async (data: IFormInput) => {
        const fundService = new FundService();

        console.log("submit pressed")
        
        console.log(data.associationId)
            var res = await fundService.add({
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

        // let errorMessage = "Association id not found";
    };

    return (
        <>
            <h1>Create</h1>

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
                                    required: true,
                                })}
                                className="form-control"
                            >   
                                <option  selected disabled hidden>***</option>
                                {associations.map((item) => {
                                    return <option key={item.id} value={item.id}>{item.name.en}</option>;
                                })}
                            </select>
                            <div className="text-danger form-text">
                                {errors.associationId?.type === "required" &&
                                    "Association is required"}
                            </div>
                        </div>
                        <div className="form-group pt-1">
                            <input
                                type="submit"
                                value="Create"
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

            <div></div>
        </>
    );
}

export default FundCreate;
