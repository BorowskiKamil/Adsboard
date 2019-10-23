using System.Threading.Tasks;
using System.Linq;
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
    public class CategoryRemovedHandler : IEventHandler<CategoryRemoved>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Category> _categoryRepository;
        private readonly DbSet<Advert> _advertRepository;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<CategoryRemovedHandler> _logger;

        public CategoryRemovedHandler(ApplicationContext dbContext,
            IBusPublisher busPublisher, ILogger<CategoryRemovedHandler> logger)
        {
            _dbContext = dbContext;
            _categoryRepository = dbContext.Set<Category>();
            _advertRepository = dbContext.Set<Advert>();
            _busPublisher = busPublisher;
            _logger = logger;
        }

        public async Task HandleAsync(CategoryRemoved @event, ICorrelationContext context)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.Id == @event.Id);
            if (category == null)
            {
                throw new AdsboardException(Codes.CategoryNotFound, 
                    $"Category with given id: {@event.Id} was not found.");
            }

            var adverts = await _advertRepository.Where(x => x.Category == category.Id).ToListAsync();
            if (adverts != null && adverts.Count() > 0)
            {
                _dbContext.RemoveRange(adverts);
            }

            _dbContext.Remove(category);

            var result = await _dbContext.SaveChangesAsync();

            if (result == 0)
            {
                throw new AdsboardException(Codes.RemovingCategoryError, 
                    $"An error occured during removing category.");
            }
        }
    }
}