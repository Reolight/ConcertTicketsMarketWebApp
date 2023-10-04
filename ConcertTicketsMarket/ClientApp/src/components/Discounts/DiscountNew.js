import {
    Button,
    Card,
    CardActions,
    CardContent,
    Checkbox,
    FormControlLabel,
    Grid,
    Stack,
    TextField
} from "@mui/material";
import {DateTimePicker} from "@mui/x-date-pickers";
import dayjs from "dayjs";

export default function DiscountNew(props){
    const { promo, handleRemove, onChange, index } = props;
    
    return(<Card sx={{elevation: 5}} variant='elevation'>
        <CardContent>
            <Grid direction='column' spacing={1} container>
                <Grid item><Stack direction='row' spacing={1}>
                    <TextField
                        label='Promotion code sequence'
                        value={promo.promocode}
                        onChange={e => onChange(index, 'promocode', e.target.value)}
                    />
                    <TextField
                        label='Discount value'
                        type='number'
                        value={promo.value}
                        onChange={e => onChange(index, 'value', e.target.value)}
                    />
                </Stack></Grid>
                <Grid item><Stack direction='row' spacing={1}>
                    <FormControlLabel control={
                        <Checkbox 
                            checked={promo.isAbsolute}
                            onChange={e => onChange(index, 'isAbsolute', e.target.checked)}
                        />}
                                      label="Absolute value"
                    />
                    <FormControlLabel control={
                        <Checkbox
                            checked={promo.hasExpirationDate}
                            onChange={e => 
                                onChange(index,
                                    {
                                        'hasExpirationDate': e.target.checked,
                                        'endTime': e.target.checked
                                            ? dayjs()
                                            : '__rem'
                                    })}
                        />}
                                      label="Does have expiration date"
                    />
                </Stack> </Grid>
                <Grid item><Stack direction='row' spacing={1} >
                    <DateTimePicker
                        label="Valid from"
                        value={promo.startTime}
                        onChange={e => onChange(index, 'startTime', e)}
                    />
                    {promo.hasExpirationDate && !!promo.endTime && <DateTimePicker
                        label="Valid until"
                        value={promo.endTime}
                        onChange={e => onChange(index, 'endTime', e)}
                    />}
                </Stack></Grid>
            </Grid>
        </CardContent>

        <CardActions>
            <Button color='error' onClick={() => handleRemove(index)}>
                Remove
            </Button>
        </CardActions>
    </Card> );
}