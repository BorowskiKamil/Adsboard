using System.Threading.Tasks;
using Adsboard.Common.Handlers;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.Types;
using Adsboard.Services.Adverts.Data;
using Adsboard.Services.Adverts.Domain;
using Adsboard.Services.Adverts.Infrastructure;
using Adsboard.Services.Adverts.Messages.Commands;
using Adsboard.Services.Adverts.Messages.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Adsboard.Services.Adverts.Handlers.Adverts
{
    public class UpdateAdvertHandler : ICommandHandler<UpdateAdvert>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Advert> _advertRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<UpdateAdvertHandler> _logger;

        public UpdateAdvertHandler(ApplicationContext dbContext,
            IBusPublisher busPublisher, ILogger<UpdateAdvertHandler> logger)
        {
            _dbContext = dbContext;
            _advertRepository = dbContext.Set<Advert>();
            _busPublisher = busPublisher;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateAdvert command, ICorrelationContext context)
        {
            var advert = await _advertRepository.FirstOrDefaultAsync(x => x.Id == command.Id &&
                x.Creator == command.UserId && !x.ArchivedAt.HasValue);
            
            if (advert == null)
            {
                _logger.LogWarning($"Couldn't find any advert matching given id: '{command.Id}'.");
                throw new AdsboardException(Codes.AdvertNotFound, 
                    $"Couldn't find any advert matching given id: '{command.Id}'");
            }

            if (!string.IsNullOrEmpty(command.Title))
            {
                advert.UpdateTitle(command.Title);
            }

            if (!string.IsNullOrEmpty(command.Description))
            {
                advert.UpdateDescription(command.Description);
            }

            _dbContext.Update(advert);
            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0)
            {
                _logger.LogWarning($"There was an error during saving advert with id: '{command.Id}'.");
                throw new AdsboardException(Codes.SavingAdvertError, 
                    $"There was an error during saving advert with id: '{command.Id}");
            }

            await _busPublisher.PublishAsync(new AdvertUpdated(command.Id, command.UserId), context);
        }
    }
}