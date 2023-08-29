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

export function initPerformerUpdate(performer = undefined){
    if (performer !== undefined){
        return {
            performer: performer,
            isNew: false
        };
    }
    
    return getNewPerformer();
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