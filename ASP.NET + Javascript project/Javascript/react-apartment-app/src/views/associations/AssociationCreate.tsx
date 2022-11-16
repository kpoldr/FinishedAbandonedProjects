import { SubmitHandler, useForm } from "react-hook-form";
import { Link, useNavigate } from "react-router-dom";
import { AssociationService } from "../../services/AssociationService";
import { useAppSelector } from "../../store/hooks";

interface IFormInput {
    name: string;
    description: string;
    email: string;
    phone: number;
    bankName: string;
    bankNumber: string;
}

function AssociationCreate() {
    
    const jwt = useAppSelector((state) => state.identity);
    const navigate = useNavigate();

    const {
        register,
        formState: { errors },
        handleSubmit,
    } = useForm<IFormInput>();

    const onSubmit: SubmitHandler<IFormInput> = (data) => TryCreate(data);

    const TryCreate = async (data: IFormInput) => {
        const associationService = new AssociationService();
        

        var res = await associationService.add({
            name: JSON.parse(`{"en": "${data.name}"}`),
            description:  JSON.parse(`{"en": "${data.description}"}`),
            email: data.email,
            phone: data.phone,
            bankName:  JSON.parse(`{"en": "${data.bankName}"}`),
            bankNumber: data.bankNumber,
            appUserId: jwt.appUserId
          });
        
          console.log(res)
        if (res.status >= 300 && res.errorMessage) {
            // let errorMessage = res.status + " " + res.errorMessage;
        
        } else {
        
            navigate("/Associations");
            
        }

        console.log(jwt);
    };


    return (
        <>
            <h1>Create</h1>

            <h4>Association</h4>
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
                                    value="Create"
                                    className="btn btn-primary"
                                />
                                <Link
                                to={`/Associations`}
                                className="btn btn-primary m-1"
                            >
                                Back to List{" "}
                            </Link>{" "}
                        </div>
                    </div>
                </form>
            </div>

            <div>
            
            </div>
        </>
    );
}

export default AssociationCreate;

