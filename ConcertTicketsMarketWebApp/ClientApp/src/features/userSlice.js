import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'

const initialState = {
    user: undefined
}

export const tryLogin = createAsyncThunk("user/getUser", async () => {
    const user = await authService.getUser();
    console.log("user object: ", user);
    return user;
})

export const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        update(state, action){
            state.user = action.payload;
        }
    },
    extraReducers: builder => {
        builder
            .addCase(tryLogin.fulfilled, (state, action) => {
                state.user = action.payload;
            })
    }
});

export const { update } = userSlice.actions;

export default userSlice.reducer;