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
    public class ArchiveAdvertHandler : ICommandHandler<ArchiveAdvert>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Advert> _advertRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<ArchiveAdvertHandler> _logger;

        public ArchiveAdvertHandler(ApplicationContext dbContext,
            IBusPublisher busPublisher, ILogger<ArchiveAdvertHandler> logger)
        {
            _dbContext = dbContext;
            _advertRepository = dbContext.Set<Advert>();
            _busPublisher = busPublisher;
            _logger = logger;
        }

        public async Task HandleAsync(ArchiveAdvert command, ICorrelationContext context)
        {
            var advert = await _advertRepository.FirstOrDefaultAsync(x => x.Id == command.Id &&
                x.Creator == command.UserId && !x.ArchivedAt.HasValue);
            
            if (advert == null)
            {
                _logger.LogWarning($"Couldn't find any advert matching given id: '{command.Id}'.");
                throw new AdsboardException(Codes.AdvertNotFound, 
                    $"Couldn't find any advert matching given id: '{command.Id}'");
            }

            advert.Archive();
            _dbContext.Update(advert);

            var result = await _dbContext.SaveChangesAsync();
            if (result <= 0)
            {
                _logger.LogWarning($"There was an error during saving advert with id: '{command.Id}'.");
                throw new AdsboardException(Codes.SavingAdvertError, 
                    $"There was an error during saving advert with id: '{command.Id}");
            }

            await _busPublisher.PublishAsync(new AdvertArchived(command.Id, command.UserId, advert.ArchivedAt.Value), context);
        }
    }
}