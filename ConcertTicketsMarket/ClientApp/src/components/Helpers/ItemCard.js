import { Button, Typography, Card, CardActionArea, CardContent, CardActions, CardHeader, Paper, Stack } from "@mui/material";

export default function ItemCard({ name, description, additional, picture, primary_action }){
    return (<Paper 
        elevation={8} 
        key={props.item.id} 
        style={props.style} 
        sx={{my: 2, mx: 2, p: 2, borderRadius: 2}}
    >
        <Stack direction={'column'} alignContent={'space-evenly'}>
            <Typography gutterBottom variant="h5" component="div">
                {typeof props.name === 'function'?
                    props.item.name() : props.item.name}
            </Typography>

            <Typography variant="body2" color="text.secondary">
                {typeof props.description === "function"?
                    props.item.description() : props.item.description}
            </Typography>
        </Stack>
    </Paper>)   // add rest.
}