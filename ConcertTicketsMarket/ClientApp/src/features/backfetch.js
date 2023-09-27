import authService from '../components/api-authorization/AuthorizeService'

function build_params(params){
    let query;
    if (!params) return '';
    for (const prop_name in params){
        query += `${(!query? '?' : '&')}${prop_name}=${params[prop_name]}}`;
    }

    return query;
}

async function make_request(querystring, method, object, content_type, stringify = true) {
    let req = {};
    const token = await authService.getAccessToken();
    if (!!token)
        req = {...req, headers: {Authorization: `Bearer ${token}`}};
    if (!!content_type)
        req = {...req, headers: { ...req.headers, ['Content-type']: content_type}};
        req = {...req, method: !!method? method : 'get'};
    if (!!object)
        req = {...req, body: stringify? JSON.stringify(object) : object};

    console.log(`fetching: `, querystring, req);
    return fetch('/api/' + querystring, req);
}

export async function Get(route, params_obj /*is obj*/){
    console.log(`get from: `, route, params_obj)
    const query = build_params(params_obj);
    const qs = `${route}${route.at(-1) === '/'? '' : '/'}${query}`;
    const response = await make_request(qs);
    console.log(`response from ${qs}: `, response);
    if (!response.ok){
        console.error(response.error);
        return;
    }

    const data = response.json();
    return data;
}

export async function Post(address, object, stringify = true, onDone /*func*/){
    const response = await make_request(address, `post`, object, 'application/json', stringify)
    onDone !== null && onDone();
    if (!response.ok)
        return await response.error;
    return await response.json();
}

export async function Update(address, object, stringify = true){
    const response = await make_request(address, `update`, object, 'application/json', stringify);
    return response;
}