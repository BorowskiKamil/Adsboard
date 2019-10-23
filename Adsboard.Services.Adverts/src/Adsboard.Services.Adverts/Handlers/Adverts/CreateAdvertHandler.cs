using System.Threading.Tasks;
using Adsboard.Common.Handlers;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.Types;
using Adsboard.Services.Adverts.Data;
using Adsboard.Services.Adverts.Domain;
using Adsboard.Services.Adverts.Messages.Commands;
using Adsboard.Services.Adverts.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Adsboard.Services.Adverts.Messages.Events;

namespace Adsboard.Services.Adverts.Handlers.Adverts
{
    public class CreateAdvertHandler : ICommandHandler<CreateAdvert>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Advert> _advertRepository;
        private readonly DbSet<User> _userRepository;
        private readonly DbSet<Category> _categoryRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<CreateAdvertHandler> _logger;

        public CreateAdvertHandler(ApplicationContext dbContext,
            IBusPublisher busPublisher, ILogger<CreateAdvertHandler> logger)
        {
            _dbContext = dbContext;
            _advertRepository = dbContext.Set<Advert>();
            _userRepository = dbContext.Set<User>();
            _categoryRepository = dbContext.Set<Category>();
            _busPublisher = busPublisher;
            _logger = logger;
        }

        public async Task HandleAsync(CreateAdvert command, ICorrelationContext context)
        {
            var category = _categoryRepository.FirstOrDefaultAsync(x => x.Id == command.CategoryId);
            if (category == null)
            {
                throw new AdsboardException(Codes.CategoryNotFound, $"Couldn't find category with given id: {command.CategoryId}.");
            }

            var user = _userRepository.FirstOrDefaultAsync(x => x.Id == command.UserId);
            if (user == null)
            {
                throw new AdsboardException(Codes.UserNotFound, $"Couldn't find user with given id: {command.UserId}.");
            }

            var advert = new Advert(command.Id, command.Title, command.Description,
                command.UserId, command.CategoryId, command.Image);

            _dbContext.Add(advert);
            var saveResult = await _dbContext.SaveChangesAsync();

            if (saveResult == 0) 
            {
                throw new AdsboardException(Codes.SavingAdvertError, "An error occured during saving advert.");
            }

            await _busPublisher.PublishAsync(new AdvertCreated(command.Id, command.UserId), context);
        }
    }
}