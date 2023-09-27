import { useEffect, useState } from "react";
import { AdvancedQueryBuilder, DEFAULT_PAGE_COUNT } from "../../features/advancedQueryBuilder";
import { useNavigate } from "react-router-dom";
import Loading from "./Loading";
import { List, ListItem, ListItemButton, ListItemText } from "@mui/material";
import { Get } from "../../features/backfetch";

export default function SimplifiedList(props){
    const {route, collection_name, handlePicked} = props;

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
    
    if (state.isLoading) return <Loading/>;
    
    return(<List sx={{ pt: 0 }}>
        {
        state[collection_name] &&
        state[collection_name].length > 0 && 
        state[collection_name].map(element =><>
            <ListItem key={element.id} >
                <ListItemButton onClick={() => handlePicked(element)}>
                    <ListItemText
                        primary={typeof props.name === 'function'
                            ? props.name(element)
                            : element[props.name]
                        }
                        secondary={typeof props.description === 'function'
                            ? props.description(element)
                            : element[props.description]
                        } 
                    />
                </ListItemButton>
            </ListItem>
        </>)}
    </List>);
}