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
        private readonly DbSet<Category> _advertRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<CreateCategoryHandler> _logger;

        public CreateCategoryHandler(ApplicationContext dbContext,
            IBusPublisher busPublisher, ILogger<CreateCategoryHandler> logger)
        {
            _dbContext = dbContext;
            _advertRepository = dbContext.Set<Category>();
            _busPublisher = busPublisher;
            _logger = logger;
        }

        public async Task HandleAsync(CreateCategory command, ICorrelationContext context)
        {
            var advert = new Category(command.Id, command.Name, command.UserId);

            _dbContext.Add(advert);
            var saveResult = await _dbContext.SaveChangesAsync();

            if (saveResult == 0) 
            {
                throw new AdsboardException(Codes.SavingCategoryError, "An error occured during saving category.");
            }

            await _busPublisher.PublishAsync(new CategoryCreated(command.Id, command.Name, command.UserId), context);
        }
    }
}