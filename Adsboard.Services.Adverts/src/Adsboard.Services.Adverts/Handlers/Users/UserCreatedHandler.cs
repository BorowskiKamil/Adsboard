using System.Threading.Tasks;
using Adsboard.Common.Handlers;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.Types;
using Adsboard.Services.Adverts.Data;
using Adsboard.Services.Adverts.Domain;
using Adsboard.Services.Adverts.Infrastructure;
using Adsboard.Services.Users.Messages.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Adsboard.Services.Adverts.Handlers.Users
{
    public class UserCreatedHandler : IEventHandler<UserCreated>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<User> _userRepository;

        public UserCreatedHandler(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
            _userRepository = dbContext.Set<User>();
        }

        public async Task HandleAsync(UserCreated @event, ICorrelationContext context)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == @event.Id);
            if (user != null)
            {
                throw new AdsboardException(Codes.UserAlreadyExists, 
                    $"User with given id: {@event.Id} already exists.");
            }

            user = new User(@event.Id, @event.Email);
            _dbContext.Add(user);

            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
            {
                throw new AdsboardException(Codes.SavingUserError, 
                    $"An error occured during saving user.");
            }
        }
    }
}