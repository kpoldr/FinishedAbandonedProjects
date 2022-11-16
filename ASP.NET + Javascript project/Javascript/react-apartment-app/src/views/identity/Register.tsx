import { watch } from "fs";
import React, { useContext, useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { IdentityService } from "../../services/IdentityService";
import { useAppDispatch, useAppSelector } from "../../store/hooks";
import { update } from "../../store/identity";
// import { AppContext } from "../state/AppContext";

interface IFormInput {
    email: string;
    firstName: string;
    lastName: string;
    password: string;
    confirmPassword: string;
}

function Register() {
    const {
        register,
        formState: { errors },
        handleSubmit,
        watch,
    } = useForm<IFormInput>();

    const onSubmit: SubmitHandler<IFormInput> = (data) => TryRegister(data);
    const jwt = useAppSelector((state) => state.identity);
    const dispatch = useAppDispatch();
    const navigate = useNavigate();

    const TryRegister = async (data: IFormInput) => {
        let identityService = new IdentityService();
        

        var res = await identityService.register(data.email, data.password, data.firstName, data.lastName);

        if (res.status >= 300 && res.errorMessage) {
            let errorMessage = res.status + " " + res.errorMessage;
        } else {
            
            var res = await identityService.login(data.email, data.password);

            if (res.status === 200) {
                console.log(res.data!);
                dispatch(update(res.data!));
                navigate("/");
            } else {
                let errorMessage = res.status + " " + res.errorMessage;
            }
        }

        console.log(jwt);
    };

    return (
        <>
            <div className="col-md-8">
                <h1>Register</h1>

                <hr />
                <div className="row">
                    <form className="col-md-6" onSubmit={handleSubmit(onSubmit)}>
                        <div>
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
                            <div className="row">
                                <div className="col">
                                    <div className="form-group">
                                        <label className="control-label">
                                            First Name
                                        </label>
                                        <input
                                            {...register("firstName", {
                                                required: true,
                                            })}
                                            className="form-control"
                                            type="text"
                                        />
                                        <div className="text-danger form-text">
                                            {errors.firstName?.type ===
                                                "required" &&
                                                "First Name is required"}
                                        </div>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-group">
                                        <label className="control-label">
                                            Last Name
                                        </label>
                                        <input
                                            {...register("lastName", {
                                                required: true,
                                            })}
                                            className="form-control"
                                            type="text"
                                        />
                                        <div className="text-danger form-text">
                                            {errors.lastName?.type ===
                                                "required" &&
                                                "Last Name is required"}
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="form-group">
                                <label className="control-label">
                                    Password
                                </label>
                                <input
                                    {...register("password", {
                                        required: true,
                                    })}
                                    className="form-control"
                                    type="password"
                                />
                                <div className="text-danger form-text">
                                    {errors.password?.type === "required" &&
                                        "Last Name is required"}
                                </div>
                            </div>
                            <div className="form-group">
                                <label className="control-label">
                                    Confirm Password
                                </label>
                                <input
                                    {...register("confirmPassword", {
                                        required: true
                                        
                                    })}
                                    className="form-control"
                                    type="password"
                                />
                                <div className="text-danger form-text">
                                    {errors.password?.type === "required" &&
                                        "Last Name is required"}
                                    {watch('password') !== watch('confirmPassword') &&
                                        "Password doesn't match"}
                                </div>
                            </div>
                            <div className="form-group pt-3">
                                <input
                                    type="submit"
                                    value="Create"
                                    className="btn btn-primary"
                                />
                            </div>
                        </div>
                    </form>
                </div>

                <div>
                    <a href="/Persons">Back to List</a>
                </div>
            </div>
        </>
    );
}

export default Register;
