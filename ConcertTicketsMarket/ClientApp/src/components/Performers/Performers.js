import { RouteParts } from "../../AppRoutes";
import DataList from "../Helpers/DataList";

// props has user

export default function Performers({ user }){
    const performer_linker = (performer, prop_name) => {
        switch (prop_name){
            case 'name':
                return performer.name;
            case 'description':
                return `Origin from ${performer.origin}`;
            case 'primary_action':
                return () => console.log(`clicked on: `, performer);
            default:
                return '';
        };
    }
    
    return <DataList 
        route={RouteParts.performers} 
        collection_name={`performers`}
        object_linker={performer_linker}
    />
}