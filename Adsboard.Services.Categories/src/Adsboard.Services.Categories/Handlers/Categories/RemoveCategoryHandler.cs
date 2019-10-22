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
    public class RemoveCategoryHandler : ICommandHandler<RemoveCategory>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Category> _categoryRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<RemoveCategoryHandler> _logger;

        public RemoveCategoryHandler(ApplicationContext dbContext,
            IBusPublisher busPublisher, ILogger<RemoveCategoryHandler> logger)
        {
            _dbContext = dbContext;
            _categoryRepository = dbContext.Set<Category>();
            _busPublisher = busPublisher;
            _logger = logger;
        }

        public async Task HandleAsync(RemoveCategory command, ICorrelationContext context)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.Id == command.Id && x.Creator == command.UserId);
            if (category == null)
            {
                throw new AdsboardException(Codes.CategoryNotFound, 
                    $"Couldn't find category with id: {category.Id}.");
            }

            _dbContext.Remove(category);
            var saveResult = await _dbContext.SaveChangesAsync();

            if (saveResult == 0) 
            {
                throw new AdsboardException(Codes.RemovingCategoryError, "An error occured during removing category.");
            }

            await _busPublisher.PublishAsync(new CategoryRemoved(command.Id, command.UserId), context);
        }
    }
}