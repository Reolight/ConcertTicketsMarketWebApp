import DataList from "../Helpers/DataList";

export default function Performers(props){
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
        route={`performers/` } 
        object_linker={performer_linker}
    />
}