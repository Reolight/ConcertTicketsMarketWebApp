import { useEffect, useState } from "react";
import { Performer } from "../../types/Performers";
import editedInstance from "../../additional/editedInstance";
import { Box, TextField, Typography } from "@mui/material";


export default function PerformerUpdate({ performer }){

    const [erorrs, setErrors] = useState(['Fill required fields']);
    const [state, setState] = useState();

    useEffect(() => {
        if (!props.performer) 
        {
            setState(
                {
                    performer: {
                        concerts: [],
                        id: '',
                        name: '',
                        origin: ''
                    },
                    
                    isNew: true,
                }
            );

            return;
        }

        setState( { performer: props.performer, isNew: false })
    })

    if (!state)
        return <p>Loading...</p>;
    else return (
        <Box component={'form'}>
            <TextField id="performer_name" label="Performer name" variant="outlined" 
                onChange={(e) => handleNameInput(e)}/>
        </Box>)

    const handleNameInput = e => {
        setState(produce(draftState => {
            draftState.performer.name = e.target.value;
        }));
    }
}