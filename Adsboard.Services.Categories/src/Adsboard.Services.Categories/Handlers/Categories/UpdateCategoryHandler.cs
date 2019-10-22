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
    public class UpdateCategoryHandler : ICommandHandler<UpdateCategory>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Category> _categoryRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<UpdateCategoryHandler> _logger;

        public UpdateCategoryHandler(ApplicationContext dbContext,
            IBusPublisher busPublisher, ILogger<UpdateCategoryHandler> logger)
        {
            _dbContext = dbContext;
            _categoryRepository = dbContext.Set<Category>();
            _busPublisher = busPublisher;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateCategory command, ICorrelationContext context)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (category == null)
            {
                throw new AdsboardException(Codes.CategoryNotFound, 
                    $"Couldn't find category with id: {category.Id}.");
            }

            if (!string.IsNullOrEmpty(command.Name))
            {
                category.UpdateName(command.Name);
            }

            _dbContext.Update(category);
            var saveResult = await _dbContext.SaveChangesAsync();

            if (saveResult == 0) 
            {
                throw new AdsboardException(Codes.SavingCategoryError, "An error occured during saving category.");
            }

            await _busPublisher.PublishAsync(new CategoryUpdated(command.Id, command.Name, command.UserId), context);
        }
    }
}