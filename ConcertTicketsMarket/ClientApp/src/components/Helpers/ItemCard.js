import { Button, Typography, Card, CardActionArea, CardContent, CardActions, CardHeader, Paper, Stack, Grid, IconButton, Icon } from "@mui/material";
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { useSelector } from "react-redux";
import { Route, useNavigate } from "react-router-dom";
import { RouteParts } from "../../AppRoutes";

export default function ItemCard(props){
    const { id, name, description, additional, picture, primary_action, is_redactor } = props;
    const user = useSelector(store => store.user.user);
    const navigate = useNavigate();
    
    return (<Card 
        elevation={8} 
        key={id} 
        sx={{my: 4, mx: 4, p: 2, borderRadius: 2}}
    >
        <CardActionArea>
            <CardContent /*direction={'column'} alignContent={'space-evenly'}*/>
                <Typography gutterBottom variant="h5" component="div">
                    {typeof props.name === 'function'?
                        props.name() : props.name}
                </Typography>

                <Typography variant="body2" color="text.secondary">
                    {typeof props.description === "function"?
                        props.description() : props.description }
                </Typography>
            </CardContent>
        </CardActionArea>

        {!!user && user.role === "admin" && <CardActions>
            <IconButton onClick={() => navigate(`./${id}${RouteParts.update}`)}>
                <EditIcon />
            </IconButton>
            <IconButton onClick={() => console.warn(`Delete pressed for item ${id}`)}>
                <DeleteIcon />
            </IconButton>
        </CardActions>}
    </Card>)   // add rest.
}