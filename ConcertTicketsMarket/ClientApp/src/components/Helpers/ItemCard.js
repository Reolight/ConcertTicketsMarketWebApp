import { Button, Typography, Card, CardActionArea, CardContent, CardActions, CardHeader, Paper, Stack, Grid, IconButton } from "@mui/material";
import { DeleteIcon, EditIcon } from '@mui/icons-material/Delete'

export default function ItemCard(props){
    const { id, name, description, additional, picture, primary_action, is_redactor } = props;

    return (<Paper 
        elevation={8} 
        key={id} 
        sx={{my: 4, mx: 4, p: 2, borderRadius: 2}}
    >
        <Stack direction={'column'} alignContent={'space-evenly'}>
            <Typography gutterBottom variant="h5" component="div">
                {typeof props.name === 'function'?
                    props.name() : props.name}
            </Typography>

            <Typography variant="body2" color="text.secondary">
                {typeof props.description === "function"?
                    props.description() : props.description }
            </Typography>
            {is_redactor && <Grid container spacing={2} justifyContent='flex-end' >
                <Grid item xs={1}>
                    <IconButton >
                        <EditIcon />
                    </IconButton>
                    <IconButton >
                        <DeleteIcon/>
                    </IconButton>
                </Grid>
            </Grid>}
        </Stack>
    </Paper>)   // add rest.
}