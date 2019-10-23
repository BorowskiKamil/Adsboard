using System.Threading.Tasks;
using Adsboard.Common.Handlers;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.Types;
using Adsboard.Services.Adverts.Data;
using Adsboard.Services.Adverts.Domain;
using Adsboard.Services.Adverts.Infrastructure;
using Adsboard.Services.Adverts.Messages.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Adsboard.Services.Adverts.Handlers.Categories
{
    public class CategoryCreatedHandler : IEventHandler<CategoryCreated>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Category> _categoryRepository;

        public CategoryCreatedHandler(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
            _categoryRepository = dbContext.Set<Category>();
        }

        public async Task HandleAsync(CategoryCreated @event, ICorrelationContext context)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.Id == @event.Id);
            if (category != null)
            {
                throw new AdsboardException(Codes.CategoryAlreadyExists, 
                    $"Category with given id: {@event.Id} already exists.");
            }

            category = new Category(@event.Id, @event.UserId);
            _dbContext.Add(category);

            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
            {
                throw new AdsboardException(Codes.SavingCategoryError, 
                    $"An error occured during saving category.");
            }
        }
    }
}