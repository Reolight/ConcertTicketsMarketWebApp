// props for performer view means:
// type = 'c' if common (navigations), 'p' = pick - when need to pick smth.
// callback? = invoked, if type not 'c' used;

import { Container, Grid, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { Get } from "../../features/backfetch";
import { RouteParts } from "../../AppRoutes";

export default function PerformerView({ performer : { id, name, performerType }}) {
    const ROUTE = `${RouteParts.performers}${performer.id}`;

    const [performer, setPerformer] = useState(props.performer);

    useEffect(() => {
        if (props.performer.origin != null)
            return;
        Get(ROUTE).then(data => 
            setPerformer(data)
        );
    } ,[props])

    return(<Container>
        <Grid container spacing={1} direction="column">
            <Grid item container direction='row' justifyContent='space-evenly'>
                <Grid item>
                    <Typography>
                        {performer.name}
                    </Typography>
                </Grid>
                <Grid item>
                    <Typography variant="h5">
                        {performer.origin}
                    </Typography>
                </Grid>
            </Grid>

            {performer.voiceType &&<Grid item>
                <Typography>Voice type: {performer.voiceType}</Typography>
            </Grid>}
        </Grid>
    </Container>)
}