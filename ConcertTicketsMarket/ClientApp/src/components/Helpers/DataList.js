import { QueryBuilder } from "@mui/icons-material";
import { useEffect, useState } from "react";
import { AdvancedQueryBuilder, DEFAULT_PAGE_COUNT } from "../../features/advancedQueryBuilder";
import authService from "../api-authorization/AuthorizeService";
import { Get } from "../../features/backfetch";
import Paginator from "./Paginator";
import Loading from "./Loading";
import ItemCard from "./ItemCard";

// object linker is a function linking object properties to ItemCard values:
// example: (element, prop) => switch ... case 'name': element.name — is a linker 
// called as: <ItemCard name={object_linker(element, 'name') } ... />

export default function DataList(props) {
    const { route, collection_name, object_linker, admin_header} = props;

    const [state, setState] = useState({ [collection_name]: [], max_pages: 0, isLoading: true});
    const [user, setUser] = useState({
        [collection_name]: [],
        max_pages: 0,
        current_page: 0,
        elem_count: DEFAULT_PAGE_COUNT,
        isLoading: true
    });
    
    const query_builder = new AdvancedQueryBuilder();
    const [query, setQuery] = useState(query_builder.getQuery());

    useEffect(() => {
        async function load_data(){
            const u = await authService.getUser();
            setUser(u)
        }

        load_data();
    }, [props])

    useEffect(() => {
        console.log(`collection name is: ${collection_name}`)
        console.log(`retrieving performers`);
        Get(route, Object.keys(query).length > 0?
             query_builder.buildQuery() : undefined
        ).then(result_data => {
            console.log(result_data);
            setState({
                [collection_name]: result_data[collection_name],
                max_pages: result_data.maxPages,
                current_page: !!query.page ? query.page : 0,
                elem_count: !!query.count ? query.count : DEFAULT_PAGE_COUNT,
                isLoading: false 
            })}
        );
    }, [state.current_page, state.elem_count])

    useEffect(() => console.log(state),[state])

    if (state.isLoading) return <Loading />

    return(<>
        {!state.isLoading && 
            state[collection_name] &&
            state[collection_name].length > 0 && 
            state[collection_name].map(element => <ItemCard 
                id={element.id}
                is_redactor={!!admin_header}
                name={props.object_linker(element, 'name')}
                description={props.object_linker(element, 'description')}
                primary_action={props.object_linker(element, 'primary_action')}
                key={element.id}
            />
        )}

        {state.max_pages > 1 && <Paginator current_page={state.current_page} max_pages={state.max_pages}
            page_navigator={(page, elem_count) => {
                query_builder.changePage(page, elem_count);
            }}/>}
    </>)
}