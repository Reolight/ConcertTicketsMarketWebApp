import { createSlice } from "@reduxjs/toolkit";

const DARK =  'dark';
const LIGHT = 'light'

export default themeSlice = createSlice({
    name: 'theme',
    initialState: DARK,
    reducers: {
        toggleTheme(state) {
            state = state === DARK? LIGHT : LIGHT;
        },
        setLightTheme(state) {
            state = LIGHT;
        },
        setDarkTheme(state) {
            state = DARK;
        }
    }
})