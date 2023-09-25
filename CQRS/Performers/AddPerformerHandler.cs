using ConcertTicketsMarketModel.Data;
using ConcertTicketsMarketModel.Model.Performers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ViewModels;
using ViewModels.PostingModels;

namespace CQRS.Performers
{
    public class AddPerformerHandler : IRequestHandler<AddPerformerRequest, Performer?>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AddPerformerHandler> _logger;
        public AddPerformerHandler(AppDbContext context, ILogger<AddPerformerHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        private Performer ConvertToPerformerHierarchy(PerformerPostingModel postingModel)
            => Enum.Parse<PerformerType>(postingModel.Type, true) switch
            {
                PerformerType.Performer => new Performer
                {
                    Concerts = new(),
                    Name = postingModel.Name,
                    Origin = postingModel.Origin
                },
                PerformerType.Singer => new Singer
                {
                    Concerts = new(),
                    Name = postingModel.Name,
                    Origin = postingModel.Origin,
                    VoiceType = Enum.Parse<VoiceTypes>(postingModel!.VoiceType, true)
                },
                PerformerType.Band => new Band
                {
                    Concerts = new(),
                    Name = postingModel.Name,
                    Origin = postingModel.Origin,
                    Genre = postingModel!.Genre,
                    Performers = _context.Performers
                        .Where(performer => postingModel.Performers!.Contains(performer.Id)).ToList()
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        
        public async Task<Performer?> Handle(AddPerformerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var performer = ConvertToPerformerHierarchy(request.PostingModel);
                var performerEntry = await _context.Performers.AddAsync(performer, cancellationToken);
                _logger.LogInformation("Added {PerformerType} with name {Name}",
                    request.PostingModel.Type, request.PostingModel.Name);
                await _context.SaveChangesAsync(cancellationToken);
                return performerEntry.Entity;
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}\n{StackTrace}",
                    e.Message, e.StackTrace);
                return null;
            }
        }
    }
}
