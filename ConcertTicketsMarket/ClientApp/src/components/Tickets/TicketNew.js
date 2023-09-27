import { Button, CardActions, CardContent, Grid, TextField } from "@mui/material";
import Card from "@mui/material/Card";

export default function TicketsNew(props){
    const { ticket, onChange, index, handleRemove } = props;
    
    return(<Card sx={{elevation: 5}} variant="elevation">
        <CardContent>
            <Grid direction='row' spacing={1} container>
                <Grid item><TextField 
                    label="Description" 
                    value={ticket.description} 
                    onChange={e => onChange(index, 'description', e.target.value)}
                /></Grid>
                <Grid item><TextField
                    label='Price'
                    value={ticket.price}
                    type="number"
                    onChange={e => onChange(index, 'price', e.target.value)}
                /></Grid>
                <Grid item><TextField
                    label='Available tickets'
                    value={ticket.count}
                    type="number"
                    onChange={e => onChange(index, 'count', e.target.value)}
                /></Grid>
            </Grid>
        </CardContent>
        <CardActions>
            <Button color="error" onClick={() => handleRemove(index)}>
                Remove
            </Button>
        </CardActions>
    </Card>)
}