import { RouteParts } from "../../AppRoutes";
import DataList from "../Helpers/DataList";

export default function Concerts(props) {
    const concert_linker = (concert, prop_name) => {
        switch (prop_name){
            case 'name':
                return concert.name;
            case 'description':
                return `${concert.type}. At ${concert.startTime}. Duration: ${concert.duration}`;
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