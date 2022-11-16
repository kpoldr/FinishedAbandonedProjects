import { useForm, SubmitHandler } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { IdentityService } from "../../services/IdentityService";
import { useAppSelector, useAppDispatch } from "../../store/hooks";
import { update } from "../../store/identity";

interface IFormInput {
    email: string;
    firstName: string;
    lastName: string;
    password: string;
    confirmPassword: string;
}

function Login(): JSX.Element {
    const {
        register,
        formState: { errors },
        handleSubmit,
    } = useForm<IFormInput>();

    const onSubmit: SubmitHandler<IFormInput> = (data) => TryLogin(data);
    const jwt = useAppSelector((state) => state.identity);
    const dispatch = useAppDispatch();
    const navigate = useNavigate();

    const TryLogin = async (data: IFormInput) => {
        let identityService = new IdentityService();

        var res = await identityService.login(data.email, data.password);
        console.log(res);
        if (res.status === 200) {
            console.log("login Succesful");
            dispatch(update(res.data!));
            navigate("/");
        } else {
            console.log("login Unsuccesful");
            let errorMessage = res.status + " " + res.errorMessage;
        }

        console.log(jwt);
    };

    return (
        <>
            <h1>Login</h1>

            <hr />
            <div className="row">
                <form className="col-md-4" onSubmit={handleSubmit(onSubmit)}>
                    <div>
                        <div className="form-group">
                            <label className="control-label">Email</label>
                            <input
                                {...register("email", {
                                    required: true,
                                    minLength: 5,
                                })}
                                v-model="email"
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
                            <label className="control-label">Password</label>
                            <input
                                {...register("password", { required: true })}
                                v-model="password"
                                className="form-control"
                                type="password"
                            />
                            <div className="text-danger form-text">
                                {errors.password?.type === "required" &&
                                    "Password is required"}
                            </div>
                        </div>
                        <div className="form-group">
                            <input
                                type="submit"
                                value="Login"
                                className="btn btn-primary"
                            />
                        </div>
                    </div>
                </form>
            </div>

            <div>
                <a href="/Persons">Back to List</a>
            </div>
        </>
    );
}

export default Login;
