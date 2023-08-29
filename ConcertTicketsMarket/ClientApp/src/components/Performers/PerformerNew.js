import { useEffect, useState } from "react"
import { initPerformerUpdate, performerTypes, voiceTypes } from "../types/Performers";
import { Button, Container, Grid, TextField, breadcrumbsClasses } from "@mui/material";
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select, { SelectChangeEvent } from '@mui/material/Select';

import { produce } from 'immer'
import { Post } from "../../features/backfetch";
import { stringify } from "ajv";
//props contains performer;

export default function PerformerNew({ returnUrl, performer}) {
    const ROUTE = 'performers/';

    const [state, setState] = useState();
    const [isPosting, setPosting] = useState(false)

    useEffect(() => {
        console.log(props);
        const performer = initPerformerUpdate(props.performer);
        console.log(performer);
        setState(performer);
    }, [props])

    useEffect(() => { console.clear(); console.log(`new state: `, state)},[state])

    useEffect(() => {
        if (isPosting){
            if (!state.isNew){
                const response = Update(ROUTE, state.performer);
                console.log(response.status);
                return;
            }

            const data = Post(ROUTE, state.performer).then(resolve);
            if ('error' in data){
                console.error(data.error);
                return;
            }

            setState(produce(draft => {
                draft.performer = data.performer;
                draft.isNew = false;
            }));
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
                    <FormControl>
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