import { SubmitHandler, useForm } from "react-hook-form";
import { Link, useNavigate } from "react-router-dom";
import { OwnerService } from "../../services/OwnerService";
import { useAppSelector } from "../../store/hooks";

interface IFormInput {
    name: string;
    email: string;
    phone: number;
    advancedPay: number;
}

function OwnerCreate() {
    
    const jwt = useAppSelector((state) => state.identity);
    const navigate = useNavigate();

    const {
        register,
        formState: { errors },
        handleSubmit,
    } = useForm<IFormInput>();

    const onSubmit: SubmitHandler<IFormInput> = (data) => TryCreate(data);

    const TryCreate = async (data: IFormInput) => {
        const ownerService = new OwnerService();
        

        var res = await ownerService.add({
            name: JSON.parse(`{"en": "${data.name}"}`),
            email: data.email,
            phone: data.phone,
            advancedPay: data.advancedPay,
            appUserId: jwt.appUserId
          });
        
          console.log(res)
        if (res.status >= 300 && res.errorMessage) {
            // let errorMessage = res.status + " " + res.errorMessage;
        
        } else {
        
            navigate("/Owners");
            
        }

        console.log(jwt);
    };


    return (
        <>
            <h1>Create</h1>

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
                                    required: true,
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
                                to={`/Owners`}
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

export default OwnerCreate;

