import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "./store";
import { IAssociation } from "../domain/IAssociation";

const initialState: IAssociation[] = [];

export const associationSlice = createSlice({
    name: "association",
    // `identitySlice` will infer the state type from the `initialState` argument
    initialState,

    reducers: {
        // Use the PayloadAction type to declare the contents of `action.payload`
        Add: (state, action: PayloadAction<IAssociation>) => {
            state.push(action.payload);
        },

        Load: (state, action: PayloadAction<IAssociation[]>) => {
            
            state = []
            
            
            action.payload.forEach(association => {
                state.push(association)
            });

        }
    },
});

export const { Add, Load } = associationSlice.actions;
// Other code such as selectors can use the imported `RootState` type
export const selectAssociation = (state: RootState) => state.association;

export default associationSlice.reducer;
