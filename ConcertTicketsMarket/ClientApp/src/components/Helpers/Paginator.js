import { Grid, Button, Item } from "@mui/material";
import { useEffect, useState } from "react"

// query_maker require (page_to_query, elems_count)
export default function Paginator(props){
    // should be calculated based on viewport width...
    const { page_navigator, max_pages, current_page } = props;
    const RENDER_RANGE = 5;
    
    const [elems_count, setElemsCount] = useState(20);
    

    const [range, setRange] = useState({ 
        from: current_page - RENDER_RANGE < 0? 0 : current_page - RENDER_RANGE,
        to: current_page + RENDER_RANGE >  max_pages? max_pages : current_page + RENDER_RANGE,
        
        [Symbol.iterator]() {
            this.current = this.from;
            return this;
        },

        next(){
            if (this.current <= this.to){
                return {done: false, value: this.current++ };
            } else {
                return {done: true}
            }
        }
    })

    return(
        <footer>
            <Grid container spacing={1} justifyContent={"center"} direction={'row'}>
                <Grid item xs={1}>
                    <Button color='primary' disabled={props.index === 0} onClick={()=>props.prev}>{"<"}</Button>
                </Grid>

                <Grid item xs={10} container spacing={1} justifyContent='center'>
                    {() => {
                        {range.from > 0 && <Grid item>
                            <Button onClick={() => props.query_maker(0, elems_count)}>1</Button>
                            <Button>...</Button>
                        </Grid>}

                        for (let r of range){
                            return (
                            <Grid item>
                                <Button onClick={() => props.query_maker(r, elems_count)}>{r + 1}</Button>
                            </Grid>)
                        }

                        {range.to < props.max_pages && <Grid item>
                            <Button>...</Button>
                            <Button onClick={() => props.query_maker(props.max_pages, elems_count)}>{props.max_pages + 1}</Button>
                        </Grid>}
                    }}
                </Grid>

                <Grid item xs={1}>
                    <Button color='primary' disabled={props.current_page === props.max_pages}
                        onClick={()=>props.next}>{">"}</Button>
                </Grid>
            </Grid>
        </footer>
    )
}