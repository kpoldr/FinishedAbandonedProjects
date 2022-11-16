import { useEffect, useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { Link, useNavigate, useParams } from "react-router-dom";
import { IAssociation } from "../../domain/IAssociation";
import { AssociationService } from "../../services/AssociationService";
import { useAppSelector } from "../../store/hooks";

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

interface IFormInput {
    name: string;
    description: string;
    email: string;
    phone: number;
    bankName: string;
    bankNumber: string;
}

function AssociationEdit() {
    let { id } = useParams();
    const associationService = new AssociationService();
    const jwt = useAppSelector((state) => state.identity);
    const navigate = useNavigate();

    const [association, setAssociation] = useState(initialState);

    useEffect(() => {
        console.log("did edit");
        if (id !== undefined) {
            associationService.get(id).then((data) => setAssociation(data));

            console.log(association);
        }
        console.log(association);
    }, []);

    useEffect(() => {
        setValue("name", association.name.en);
        if (association.description !== undefined) {
            setValue("description", association.description?.en);
        }
        setValue("email", association.email);
        setValue("phone", association.phone);
        if (association.bankName !== undefined) {
            setValue("bankName", association.bankName?.en);
        }
        if (association.bankNumber !== undefined) {
            setValue("bankNumber", association.bankNumber);
        }
    }, [association]);

    const {
        register,
        formState: { errors },
        handleSubmit,
        setValue,
    } = useForm<IFormInput>();

    const onSubmit: SubmitHandler<IFormInput> = (data) => TryEdit(data);

    const TryEdit = async (data: IFormInput) => {
        const associationService = new AssociationService();

        if (id !== undefined) {
            var res = await associationService.update(id, {
                id: id,
                name: JSON.parse(`{"en": "${data.name}"}`),
                description: JSON.parse(`{"en": "${data.description}"}`),
                email: data.email,
                phone: data.phone,
                bankName: JSON.parse(`{"en": "${data.bankName}"}`),
                bankNumber: data.bankNumber,
                appUserId: jwt.appUserId,
            });

            console.log(res);
            if (res.status >= 300 && res.errorMessage) {
                // let errorMessage = res.status + " " + res.errorMessage;
            } else {
                navigate("/Associations");
            }

            console.log(jwt);
        }
    };

    return (
        <>
            <h1>Create</h1>

            <h4>Associations</h4>
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
                            <label className="control-label">Description</label>
                            <input
                                {...register("description", {
                                    required: false,
                                })}
                                className="form-control"
                                type="text"
                            />
                        </div>

                        <div className="form-group">
                            <label className="control-label">Email</label>
                            <input
                                {...register("email", {
                                    required: true,
                                })}
                                className="form-control"
                                type="text"
                            />
                            <div className="text-danger form-text">
                                {errors.email?.type === "required" &&
                                    "Email is required"}
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
                                {errors.phone?.type === "required" &&
                                    "Phone is required"}
                            </div>
                        </div>
                        <div className="form-group">
                            <label className="control-label">Bank name</label>
                            <input
                                {...register("bankName", {
                                    required: false,
                                })}
                                className="form-control"
                                type="text"
                            />
                        </div>
                        <div className="form-group">
                            <label className="control-label">Bank number</label>
                            <input
                                {...register("bankNumber", {
                                    required: false,
                                })}
                                className="form-control"
                                type="text"
                            />
                        </div>

                        <div className="form-group pt-1">
                            <input
                                type="submit"
                                value="Save"
                                className="btn btn-primary m-1"
                            />
                            <Link
                                to={`/Associations/`}
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

export default AssociationEdit;
