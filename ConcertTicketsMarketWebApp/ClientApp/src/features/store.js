import { configureStore } from "@reduxjs/toolkit";
import userReducer, { userSlice } from "./userSlice";
import concertsReducer from './concertsSlice';

const store = configureStore({
    reducer: {
        user: userReducer,
        concerts: concertsReducer
    }
})

export default store;