import { useEffect, useState } from "react"
import { getNewPerformer, performerTypes, voiceTypes } from "../verboseHandlers/Performers";
import { Button, Container, Grid, TextField } from "@mui/material";
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';

import { produce } from 'immer'
import { Get, Post, Update } from "../../features/backfetch";
import { useParams } from "react-router-dom";
import { RouteParts } from "../../AppRoutes";
//props contains performer;

export default function PerformerNew(props) {
    const ROUTE = 'performers/';
    const { id } = useParams();
    const [state, setState] = useState();
    const [isPosting, setPosting] = useState(false)

    useEffect(() => {
        console.log(`Gotten id in params: ${id}`);
        if (!!id) {
            Get(`${RouteParts.performers}/${id}`).then(
                performer => {
                    console.log(`Fetched performer: `, performer)
                    setState({
                        performer: performer,
                        isNew: false
                    })
                }
            );
        } else setState(getNewPerformer());
    }, [props])

    useEffect(() => { console.log(`new state: `, state)},[state])

    useEffect(() => {
        if (isPosting){
            if (!state.isNew){
                const response = Update(ROUTE, { performer: {}, ...state.performer });
                console.log(response.status);
                return;
            }

            Post(ROUTE, { performer: {}, ...state.performer })
                .then(data =>
                {
                    if ('error' in data) {
                        console.error(data.error);
                        setPosting(false);
                        return;
                    }

                    setPosting(false);
                    setState(produce(draft => {
                        draft.performer = data.performer;
                        draft.isNew = false;
                    }))

                });
        } else console.log(`edited?`)
    }, [isPosting])

    const handlePosting = () => {
        setPosting(true);
    }

    const handlePropertyChange = (e, field) => {
        setState(produce(draftState => {
            draftState.performer[field] = e.target.value;
            return draftState;
        }))
    }

    if (!state)
        return <p>Loading...</p>
    
    return (<>
    <Container sx={{mx: '10%'}}>
        <Grid container spacing={2} direction='column'>
            <Grid item xs={6}>
                <TextField value={state.performer.name} 
                    label="Performer name"
                    variant="outlined"
                    onChange={e => handlePropertyChange(e, 'name')}
                />
            </Grid>
            <Grid item xs={6}>
                <TextField value={state.performer.origin}
                    label="Origin"
                    variant="outlined"
                    onChange={e => handlePropertyChange(e, 'origin')} 
                />
            </Grid>
            <Grid item xs={12} container direction='column' spacing={2}>
                <Grid item xs={4}>
                    <FormControl sx={{width: '180px'}}>
                        <InputLabel id="performer-type-selection-label">Performer type</InputLabel>
                        <Select labelId="performer-type-selection-label" id="performer-type-selection"
                            value={state.performer.type}
                            label='Performer type'
                            onChange={e => handlePropertyChange(e, 'type')}
                        >
                            {mapArrayToMenuItem(performerTypes)}
                        </Select>
                    </FormControl>
                </Grid>
            
                {renderSpecificFields()}
            </Grid>
            <Grid item xs={12} spacing={3} container direction='row' justifyContent={"center"}>
                
                    {state.isNew? <>
                        <Grid item xs={3}>
                            <Button color="success" variant="contained"
                                disabled={isPosting}
                                onClick={handlePosting}
                            >
                                Create
                            </Button>
                        </Grid>
                        <Grid item xs={3}>
                            <Button color="warning" variant="contained">
                                Cancel
                            </Button>
                        </Grid>
                    </> : <>
                        <Grid item xs={3}>
                            <Button color="primary" variant="contained"
                                disabled={isPosting}
                            >
                                Update
                            </Button>
                        </Grid>
                        <Grid item xs={3}>
                            <Button color="error" variant="contained">
                                Delete
                            </Button>
                        </Grid>
                    </>}
            </Grid>
        </Grid>
    </Container>
    </>)

    function renderSpecificFields() {
        switch (state.performer.type){
            case 'band':
                return (<Grid item xs={6}>
                    <TextField value={state.performer.genre} 
                        label="Genre"
                        variant="outlined"
                        onChange={e => handlePropertyChange(e, 'genre')} 
                    />
                    <p>Here will be menu for selecting another performers</p>
                </Grid>)
            case 'singer':
                return(<Grid item xs={6}>
                    <FormControl>
                        <InputLabel id="voice-type-selection-label">Voice type</InputLabel>
                        <Select labelId="voice-type-selection-label" id="voice-type-selection"
                            label="Voice type" value={state.performer.voiceType}
                            onChange={e => handlePropertyChange(e, 'voiceType')} 
                        >
                            {mapArrayToMenuItem(voiceTypes)}
                        </Select>
                    </FormControl>
                </Grid>)
        }
    }
    
    function mapArrayToMenuItem(array){
        return array.map(type => <MenuItem key={type} value={type}>{type}</MenuItem>)
    }
}