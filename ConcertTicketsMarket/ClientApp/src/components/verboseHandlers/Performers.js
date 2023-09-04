// export interface Performer {
//     id: string;
//     name: string;
//     origin: string;
//     concerts: string[];
// }
// 
// export interface Band extends Performer {
//    genre: string;
//    performers: string[]
// }

import { RouteParts } from "../../AppRoutes";
import { Get } from "../../features/backfetch";

function fetchById(id) {
    Get(`${RouteParts.performers}/${id}`).then(
        performer => {
            console.log(`Fetched performer: `, performer)
            return {
                performer: performer,
                isNew: false
            }
        }
    );
}

export function initPerformerUpdate(id = undefined) {
    if (!!id){
        return fetchById(id)
    } else return getNewPerformer();
}

export function getNewPerformer(){
    return { performer:
        {
            id: '',
            type: performerTypes[0],
            name: '',
            origin: '',
            genre: '',
            voiceType: voiceTypes[0],
            performers: []
        },
        isNew: true
    }
}

export const performerTypes = ['performer', 'singer', 'band']
export const voiceTypes = ['Soprano', 'MezzoSoprano', 'Contralto', 'Tenor', 'Baritone', 'Bass']