import { createAsyncThunk, createEntityAdapter, createSlice } from "@reduxjs/toolkit"
import { BackRoutes } from "../AppRoutes";


const initialState = {
    entities: []
}

const concertsAdapter = createEntityAdapter();

export const fetchConcerts = createAsyncThunk(
    `concerts/fetchConcerts`,
    async (criteria) => {
        const response = 
            await fetch(`${window.location.origin}${BackRoutes.Concerts}/?criteria=${JSON.stringify(criteria)}`, 
            { method: 'GET' }
        );

        if (!response.ok)
            console.log(`fetching concerts finished with code ${response.status}`)
        const data = await response.json()
        console.log(data);
        return data;
    }
)

export const concertsSlice = createSlice({
    name: 'concerts',
    initialState,
    reducers: {

    }
})

export const {
    selectById: selectConcertById,
    selectIds: selectConcertIds,
    selectEntities: selectConcertEntities,
    selectAll: selectAllConcerts,
    selectTotal: selectTotalConcerts,    
} = concertsAdapter.getSelectors((state) => state.concerts)

export default concertsSlice.reducer;