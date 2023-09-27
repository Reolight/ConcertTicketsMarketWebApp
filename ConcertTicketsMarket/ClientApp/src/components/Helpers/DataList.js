import { useEffect, useState } from "react";
import { AdvancedQueryBuilder, DEFAULT_PAGE_COUNT } from "../../features/advancedQueryBuilder";
import { Get } from "../../features/backfetch";
import Paginator from "./Paginator";
import Loading from "./Loading";
import ItemCard from "./ItemCard";
import { useSelector } from "react-redux";
import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { RouteParts } from "../../AppRoutes";

// object linker is a function linking object properties to ItemCard values:
// example: (element, prop) => switch ... case 'name': element.name — is a linker 
// called as: <ItemCard name={object_linker(element, 'name') } ... />

export default function DataList(props) {
    const user = useSelector(store => store.user.user);
    const { route, collection_name, object_linker, update_route_prefix } = props;

    const [state, setState] = useState({
        [collection_name]: [],
        max_pages: 0,
        current_page: 0,
        elem_count: DEFAULT_PAGE_COUNT,
        isLoading: true
    });

    const query_builder = new AdvancedQueryBuilder();
    const [query, setQuery] = useState(query_builder.getQuery());
    const navigate = useNavigate();

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

    function updateRequest(id) {
        const updateRoute = (!update_route_prefix? '.': `.${update_route_prefix}`)
            + `/${id}${RouteParts.update}`;
        navigate(updateRoute);
    }

    if (state.isLoading) return <Loading />

    return(<>
        {!!user && user.role === 'admin' && <Button variant="contained" 
            onClick={() => navigate(!update_route_prefix? `.${RouteParts.new}`
                : `.${update_route_prefix}${RouteParts.new}`)} >Add {collection_name}
        </Button>}

        {state[collection_name] &&
        state[collection_name].length > 0 && 
        state[collection_name].map(element => <ItemCard 
            id={element.id}

            name={props.object_linker(element, 'name')}
            description={props.object_linker(element, 'description')}
            primary_action={props.object_linker(element, 'primary_action')}
            update_request={updateRequest}
            key={element.id}
        />)}

        {state.max_pages > 1 && <Paginator current_page={state.current_page} max_pages={state.max_pages}
            page_navigator={(page, elem_count) => {
                query_builder.changePage(page, elem_count);
        }}/>}
    </>)
}