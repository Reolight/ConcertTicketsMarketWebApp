import { RouteParts } from "../../AppRoutes";
import DataList from "../Helpers/DataList";
import dayjs from "dayjs";
import {ConcertTypes} from "../verboseHandlers/Concerts";

export default function Concerts(props) {
    const concert_linker = (concert, prop_name) => {
        switch (prop_name){
            case 'name':
                return concert.name;
            case 'description':
                return () => (<>
                    <p><b>Concert type: </b>{ConcertTypes[concert.type]}.</p>
                    <p><b>Starts at </b>{dayjs(concert.startTime)
                        
                        .format("DD MMMM YYYY HH:mm")}.</p>
                    <p><b>Duration:</b> {concert.duration} minutes</p>
                    </>)
            case 'primary_action':
                return () => console.log(`clicked on: `, concert);
            default:
                return '';
        };
    }
    
    return <DataList 
        route={RouteParts.concerts} 
        collection_name={`concerts`}
        object_linker={concert_linker}
        update_route_prefix={RouteParts.concerts}
    />
}