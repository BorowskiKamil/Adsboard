using System.Threading.Tasks;
using Adsboard.Common.Handlers;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.Types;
using Adsboard.Services.Categories.Data;
using Adsboard.Services.Categories.Domain;
using Adsboard.Services.Categories.Infrastructure;
using Adsboard.Services.Categories.Messages.Commands;
using Adsboard.Services.Categories.Messages.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Adsboard.Services.Categories.Handlers.Categories
{
    public class CreateCategoryHandler : ICommandHandler<CreateCategory>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Category> _categoryRepository;
        private readonly DbSet<User> _userRepository;
        private readonly IBusPublisher _busPublisher;

        public CreateCategoryHandler(ApplicationContext dbContext,
            IBusPublisher busPublisher)
        {
            _dbContext = dbContext;
            _categoryRepository = dbContext.Set<Category>();
            _busPublisher = busPublisher;
            _userRepository = dbContext.Set<User>();
        }

        public async Task HandleAsync(CreateCategory command, ICorrelationContext context)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == command.UserId);
            if (user == null)
            {
                throw new AdsboardException(Codes.UserNotFound, $"Couldn't find user with given id: {command.UserId}.");
            }

            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (category != null)
            {
                throw new AdsboardException(Codes.CategoryAlreadyExists, 
                    $"Category with given id: {category.Id} already exists.");
            }

            category = new Category(command.Id, command.Name, command.UserId);

            _dbContext.Add(category);
            var saveResult = await _dbContext.SaveChangesAsync();

            if (saveResult == 0) 
            {
                throw new AdsboardException(Codes.SavingCategoryError, "An error occured during saving category.");
            }

            await _busPublisher.PublishAsync(new CategoryCreated(command.Id, command.Name, command.UserId), context);
        }
    }
}