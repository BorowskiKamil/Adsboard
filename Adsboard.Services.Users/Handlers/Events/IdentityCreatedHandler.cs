using System.Threading.Tasks;
using Adsboard.Common.Handlers;
using Adsboard.Common.RabbitMq;
using Adsboard.Services.Users.Data;
using Adsboard.Services.Users.Domain;
using Adsboard.Services.Users.Messages.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Adsboard.Services.Users.Handlers.Events
{
    public class IdentityCreatedHandler : IEventHandler<IdentityCreated>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<User> _userRepository;
        private readonly ILogger<IdentityCreatedHandler> _logger;

        public IdentityCreatedHandler(ApplicationContext dbContext, ILogger<IdentityCreatedHandler> logger)
        {
            _dbContext = dbContext;
            _userRepository = _dbContext.Set<User>();
            _logger = logger;
        }

        public async Task HandleAsync(IdentityCreated @event, ICorrelationContext context)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == @event.IdentityId || x.Email == @event.Email);
            if (user != null)
            {
                _logger.LogWarning($"User with given id: {@event.IdentityId} or email: {@event.Email} already exists.");
                return;
            }

            user = new User(@event.IdentityId, @event.Email);

            _dbContext.Add(user);

            var savingResult = await _dbContext.SaveChangesAsync();
            if (savingResult == 0)
            {
                _logger.LogWarning($"There was a problem during saving user id: '{@event.IdentityId}'.");
            }
        }
    }
}