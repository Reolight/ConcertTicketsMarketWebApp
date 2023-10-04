// public string Name { get; set; } = null!;
// public ConcertType Type { get; set; }
// public DateTime StartTime { get; set; }
// public TimeSpan Duration { get; set; }
// public AgeRating Rating { get; set; }
// 
// public double Latitude { get; set; }
// public double Longitude { get; set; }
// 
// public List<TicketTemplate> Tickets { get; set; } = new();
// public Guid Performer { get; set; }
// public List<Discount> Promocodes { get; set; } = new();

import dayjs from "dayjs";

export const AgeRatings = [ 'Everyone', 'Teen', 'Mature']
export const ConcertTypes = ['Classical','Party','Open air']

export function convertToPostingModel(concert){
    return {
        ...concert,
        type: ConcertTypes.indexOf(concert.type),
        rating: AgeRatings.indexOf(concert.rating),
        performer: concert.performer.id
    }
}

export function cacheConcert(concertState) {
    if (concertState === undefined) return;
    const json = JSON.stringify(concertState);
    localStorage.setItem(`crt`, json);
}

// state from ConcertNew comes from here 
export function getCachedConcert(){
    const item = localStorage.getItem('crt');
    if (item === null) return undefined;
    const obj = JSON.parse(item);
    console.log(obj);
    const cooked_obj =  {
        ...obj,
        concert: {
            ...obj.concert,
            startTime: dayjs(obj.concert.startTime),
            
            promocodes: !!obj.concert.promocodes ? obj.concert.promocodes.map(promo => {
                const cooked = {
                    ...promo,
                    startTime: dayjs(promo.startTime)
                }

                if (promo.hasExpirationDate)
                    cooked.endTime = dayjs(promo.endTime);
                return cooked;
            }) : []
        }
    };
    
    console.log(cooked_obj);
    return cooked_obj;
}

export function createOrLoadCachedConcert(){
    const cached = getCachedConcert();
    if (!!cached) return cached;
    return getNewConcert();
}

export function getNewConcert()
{    
    return {
        concert: {
            id: '',
            name: '',
            type: ConcertTypes[0],
            startTime: dayjs(),
            duration: 0,
            rating: AgeRatings[0],

            latitude: 53.9006,
            longitude: 27.5590,

            tickets: [],
            performer: undefined,
            promocodes: []
        },

        isNew: true
    }
}