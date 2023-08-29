import { produce } from 'immer';
import authService from '../components/api-authorization/AuthorizeService'

function build_params(params){
    let query;
    for (const prop_name in params){
        query += `${(!query? '?' : '&')}${prop_name}=${params[prop_name]}}`;
    }

    return query;
}

function make_request(querystring, method, object, content_type, stringify = true) {
    let requestInit = produce(async draft_req => {
        const token = await authService.getAccessToken();
        if (!!token)
            draft_req.headers.Authorization = `Bearer ${token}`;
        if (!!content_type)
            draft_req.headers['Content-type'] = content_type;
        draft_req.method = !!method? method : 'get';
        if (!!object)
            draft_req.body = stringify? JSON.stringify(object) : object;
    });

    return fetch(querystring, requestInit);
}

export async function Get(route, params_obj /*is obj*/){
    const query = build_params(params_obj);
    const response = await make_request(`${route}${route.at(-1) === '/'? '' : '/'}${query}`);
    if (!response.ok){
        console.error(response.error);
        return;
    }

    const data = response.json();
    return data;
}

export async function Post(address, object, stringify = true, onDone /*func*/){
    const response = await this.make_request(address, `post`, object, 'application/json', stringify)
    onDone !== null && onDone();
    if (!response.ok)
        return await response.error;
    return await response.json();
}

export async function Update(address, object, stringify = true){
    const response = await this.make_request(address, `update`, object, 'application/json', stringify);
    return response;
}