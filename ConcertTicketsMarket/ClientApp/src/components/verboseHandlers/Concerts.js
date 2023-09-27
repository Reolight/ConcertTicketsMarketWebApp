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
        concertType: ConcertTypes.indexOf(concert.type),
        rating: AgeRatings.indexOf(concert.rating),
        performer: concert.performer.id,
        
        ...concert        
    }
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