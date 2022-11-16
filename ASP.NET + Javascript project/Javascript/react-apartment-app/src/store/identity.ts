import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "./store";
import { IJWTResponse } from "../domain/IJWTResponse";
import jwt_decode from "jwt-decode";


const initialState: IJWTResponse = {
    token: "",
    refreshToken: "",
    firstName: "",
    lastName: "",
    appUserId: "",
    loggedIn: false,
};

export const identitySlice = createSlice({
    name: "identity",
    // `identitySlice` will infer the state type from the `initialState` argument
    initialState,

    reducers: {
        // Use the PayloadAction type to declare the contents of `action.payload`
        update: (state, action: PayloadAction<IJWTResponse>) => {
            console.log("used update function thank")
            // state = action.payload
            state.token = action.payload.token;
            state.refreshToken = action.payload.refreshToken;
            state.firstName = action.payload.firstName;
            state.lastName = action.payload.lastName;
            let jwtToken: any = jwt_decode(action.payload.token);
            state.appUserId =
                jwtToken[
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                ];
            state.loggedIn = true;
            
        },
    },
});

export const { update } = identitySlice.actions;
// Other code such as selectors can use the imported `RootState` type
export const selectJWT = (state: RootState) => state.identity;

export default identitySlice.reducer;
