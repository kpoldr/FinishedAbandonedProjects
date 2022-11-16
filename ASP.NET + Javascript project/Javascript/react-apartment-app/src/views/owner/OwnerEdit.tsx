import { useEffect, useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { Link, useNavigate, useParams } from "react-router-dom";
import { IOwner } from "../../domain/IOwner";

import { OwnerService } from "../../services/OwnerService";
import { useAppSelector } from "../../store/hooks";

const initialState: IOwner = {
    id: "",
    name: { en: "" },
    email: "",
    phone: 0,
    advancedPay: 0,
    appUserId: "",
};

interface IFormInput {
    name: string;
    email: string;
    phone: number;
    advancedPay: number;
}

function OwnerEdit() {
    let { id } = useParams();
    const ownerService = new OwnerService();
    const jwt = useAppSelector((state) => state.identity);
    const navigate = useNavigate();

    const [owner, setOwner] = useState(initialState);

    useEffect(() => {
        if (id !== undefined) {
            ownerService.get(id).then((data) => setOwner(data));
        }
    }, []);


    useEffect(() => {
        setValue("name", owner.name.en);
        setValue("email", owner.email);
        setValue("phone", owner.phone);
        setValue("advancedPay", owner.advancedPay);
    }, [owner]);

    const {
        register,
        formState: { errors },
        handleSubmit,
        setValue,
    } = useForm<IFormInput>();

    const onSubmit: SubmitHandler<IFormInput> = (data) => TryEdit(data);

    const TryEdit = async (data: IFormInput) => {

        if (id !== undefined) {
            var res = await ownerService.update(id, {
                id: id,
                name: JSON.parse(`{"en": "${data.name}"}`),
                email: data.email,
                phone: data.phone,
                advancedPay: data.advancedPay,
                appUserId: jwt.appUserId,
            });

            console.log(res);
            if (res.status >= 300 && res.errorMessage) {
                // let errorMessage = res.status + " " + res.errorMessage;
            } else {
                navigate("/Owners");
            }

            console.log(jwt);
        }
    };

    return (
        <>
            <h1>Edit</h1>

            <h4>Owner</h4>
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
                                            {errors.name?.type ===
                                                "required" &&
                                                "Name is required"}
                                        </div>
                        </div>

                        <div className="form-group">
                        <label className="control-label">Email</label>
                                <input
                                    {...register("email", {
                                        required: true,
                                        minLength: 5,
                                    })}
                                    className="form-control"
                                    type="text"
                                />
                                <div className="text-danger form-text">
                                    {errors.email?.type === "required" &&
                                        "Email is required"}
                                    {errors.email?.type === "minLength" &&
                                        "Minimum email length is 5"}
                                </div>
                        </div>
                        <div className="form-group">
                            <label className="control-label">Phone</label>
                            <input
                            {...register("phone", {
                                required: true,
                            })}
                                className="form-control valid"
                                type="number"
                            />
                            <div className="text-danger form-text">
                                            {errors.phone?.type ===
                                                "required" &&
                                                "Phone is required"}
                                        </div>
                        </div>
                        <div className="form-group">
                            <label className="control-label">Advanced Pay</label>
                            <input
                                {...register("advancedPay", {
                                    required: false,
                                })}
                                className="form-control"
                                type="text"
                            />
                            
                        </div>
                       
                        <div className="form-group pt-2">
                        <input
                                    type="submit"
                                    value="Save"
                                    className="btn btn-primary"
                                />
                                <Link
                                to={`/Owners`}
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

export default OwnerEdit;
