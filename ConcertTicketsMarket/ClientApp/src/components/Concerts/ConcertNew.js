import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import {
    AgeRatings,
    ConcertTypes,
    getNewConcert,
    convertToPostingModel,
    createOrLoadCachedConcert, cacheConcert
} from "../verboseHandlers/Concerts";
import Loading from "../Helpers/Loading";
import { Grid, Stack, TextField, Button, Typography, FormControl, InputLabel, Select, MenuItem, Dialog } from "@mui/material";
import { produce } from "immer";
import TicketsNew from "../Tickets/TicketNew";
import { Get, Post, Update } from "../../features/backfetch";
import { RouteParts } from "../../AppRoutes";
import { DateTimePicker } from '@mui/x-date-pickers';
import YndMaps from '../Map/YndMap'
import SimplifiedList from "../Helpers/SimplifiedList";
import { performerTypes } from "../verboseHandlers/Performers";
import dayjs from "dayjs";
import DiscountNew from "../Discounts/DiscountNew";

function cachedInstance(){
    return localStorage.getItem(`crt`);
}

export default function ConcertNew(props){
    const { id } = useParams();
    const [state, setState] = useState()
    const [isPosting, setPosting] = useState(false)
    const [isPickerActive, setIsPickerActive] = useState(false)
    const [savingTimeout, setSavingTimeout] = useState(undefined);
    
    useEffect(() => {
        console.log((state));
        if (!!savingTimeout) clearTimeout(savingTimeout)
        if (!!state && !!state.concert) setSavingTimeout(setTimeout(() => cacheConcert(state), 1000));
    },[state])
    
    useEffect(() => {
        if (!!id) {
            Get(`${RouteParts.concert}/${id}`).then(
                concert => {
                    console.log(`Fetched concert: `, concert)
                    setState({
                        concert: concert,
                        isNew: false
                    })
                }
            );
        } else setState(createOrLoadCachedConcert());
    }, [props])

    useEffect(() => {
        if (isPosting){
            const concertPostingModel = convertToPostingModel(state.concert);
            
            if (!state.isNew){
                const response = Update(RouteParts.concerts, concertPostingModel)
                    .finally((res) => setPosting(false));
                console.log(response.status);
                return;
            }
    
            Post(RouteParts.concerts, concertPostingModel)
                .then(data =>
                {
                    if ('error' in data) {
                        console.error(data.error);
                        return;
                    }
    
                    setState(produce(draft => {
                        draft.concert = data.concert;
                        draft.isNew = false;
                    }))
                })
                .finally(() => setPosting(false));
        } else console.log(`edited?`)
    }, [isPosting])

    const handlePropertyChange = (value, field) => {
        setState(produce(draftState => {
            draftState.concert[field] = value;
            return draftState;
        }))
    }

    const ticketTemplate = { description: '', price: 0, count: 1 };
    const promoTemplate =  { promocode: '', value: 0.01, isAbsolute: false, startTime: dayjs(), hasExpirationDate: false, endTime: dayjs() }
    
    const addItem = (collection_name, item) => {
        setState(produce(draft => {
            draft.concert[collection_name].push(item)
        }))
    }

    const removeItem = (collection_name, index) => {
        setState(produce(draft => {
            draft.concert[collection_name] = state.concert[collection_name].filter((item, i) => index !== i);
        }))
    }

    const changePropInColl = (draft, collection_name, index, property, value) =>
    {
        if (value === '__rem')
            delete draft[collection_name][index][property];
        else
            draft[collection_name][index][property] = value;
    }
    
    const changeItem = (collection_name, index, property, value) =>{
        setState(produce(draft => {
            if (typeof property === 'object'){
                for (const prop in property){
                    changePropInColl(draft.concert, collection_name, index, prop, property[prop]);
                }
            }
            else
                changePropInColl(draft.concert, collection_name, index, property, value);
        }))
    }
    
    const changePromo = (index, property, value) => {
        setState(produce(draft => {
            draft.concert.promocodes[index][property] = value
        }))
    }

    if (!state) return <Loading/>

    return(<Grid container spacing={2} direction='column'>
        <Grid item xs={6}>
            <TextField value={state.concert.name} 
                label="Concert title"
                variant="outlined"
                onChange={e => handlePropertyChange(e.target.value, 'name')}
            />
        </Grid>

        <Grid item xs={4}>
            <FormControl sx={{width: '180px'}}>
                <InputLabel id="concert-type-selection-label">Concert type</InputLabel>
                <Select labelId="concert-type-selection-label" id="concert-type-selection"
                    value={state.concert.type}
                    label='Concert type'
                    onChange={e => handlePropertyChange(e.target.value, 'type')}
                >
                    {mapArrayToMenuItem(ConcertTypes, 0)}
                </Select>
            </FormControl>
        </Grid>

        <Grid item xs={8} spacing={2} container direction='row'>
            <Grid item xs={5}>
                <DateTimePicker
                    label="Concert start time"
                    value={state.concert.startTime}
                    onChange={e => handlePropertyChange(e, 'startTime')}
                />
            </Grid>
            <Grid item xs={3}>
                <TextField
                    variant="outlined"
                    label="Duration, mins"
                    type="number"
                    onChange={e => {
                        handlePropertyChange(Number(e.target.value), 'duration');
                    }}
                />
            </Grid>
        </Grid>

        <Grid item xs={4}>
            <FormControl sx={{width: '180px'}}>
                <InputLabel id="concert-rating-selection-label">Concert type</InputLabel>
                <Select labelId="concert-rating-selection-label" id="concert-rating-selection"
                    value={state.concert.rating}
                    label='Age rating'
                    onChange={e => handlePropertyChange(e.target.value, 'rating')}
                >
                    {mapArrayToMenuItem(AgeRatings, 1)}
                </Select>
            </FormControl>
        </Grid>

        <Grid item xs={6}>
            <Stack direction="row">
                <Typography>Performer: {!!state.concert.performer &&
                    <><b>{state.concert.performer.name}</b> <i>[{state.concert.performer.id}]</i></>}
                </Typography>
                {!isPickerActive && <Button onClick={() => setIsPickerActive(true)}>Pick performer</Button>}
            </Stack>
        </Grid>
        
        {isPickerActive && 
        <><Dialog open={isPickerActive}>
            <Grid container direction="column" >
                <Grid item xs={11} >
                    <SimplifiedList 
                        route={RouteParts.performers}
                        collection_name="performers"
                        name={(elem) => `${elem.name} (${performerTypes[elem.performerType]})`}
                        description="origin"
                        handlePicked={(elem) => {
                            setState(produce(draft => { 
                                draft.concert.performer = elem;
                                return draft;
                            }));
                            
                            setIsPickerActive(false);
                        }}
                    />
                </Grid>
                <Grid item xs={1}><Button onClick={() => setIsPickerActive(false)}>Close</Button></Grid>
            </Grid>
        </Dialog></>}

        {!isNaN(state.concert.latitude) && !isNaN(state.concert.longitude) && 
        <Grid item xs={8} >
            <YndMaps 
                defaultState={{center: [30, 50], zoom: 9}} 
                placemarks={!!state.concert.latitude && [{geometry: [state.concert.latitude, state.concert.longitude] }] }
                rest={{
                    onClick: (event) => {
                        const coords = event.get("coords");
                        setState(produce(draft => {
                            draft.concert.latitude = coords[0];
                            draft.concert.longitude = coords[1];
                            return draft;
                        }));
                    }
                }}
            />
        </Grid>
        }

        <Grid item alignSelf='center'><Button variant='outlined' onClick={() => addItem('promocodes', promoTemplate)}>
            Add discount
        </Button></Grid>
        
        {!!state.concert.promocodes.length && state.concert.promocodes.map((promo, index) => {
            return (<DiscountNew 
                key={`2.${index}`}
                index={index}
                promo={promo}
                handleRemove={(index) => removeItem('promocodes', index)}
                onChange={(index, property, value) => changeItem('promocodes', index, property, value)}
            />);
        })}
        
        <Grid item alignSelf='center'><Button variant='outlined' onClick={() => addItem('tickets', ticketTemplate)}>
            Add tickets
        </Button></Grid>
        <Stack direction='row' justifyContent='center' spacing={1}>
            {!!state.concert.tickets.length && <>
            <Typography>
                Total price: {state.concert.tickets.reduce((acc, curr) => acc += curr.price * curr.count, 0)}
            </Typography>
            <Typography>
                Tolal tickets count: {state.concert.tickets.reduce((acc, curr) => acc+=Number(curr.count), 0)}
            </Typography>
            </>}
        </Stack>

        {!!state.concert.tickets.length && state.concert.tickets.map((ticket, index) => {
            return (
                <TicketsNew ticket={ticket} 
                            key={index}
                            onChange={(index, property, value) => changeItem('tickets', index, property, value)} 
                            index={index}
                            handleRemove={(index) => removeItem('tickets', index)}
            />)
        })}


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
    </Grid>)

    function mapArrayToMenuItem(array, key_base) {
        return array.map((type, index) => <MenuItem key={`${key_base}.${index}`} value={type}>{type}</MenuItem>)
    }

    function handlePosting() {
        setPosting(true);
    }
}