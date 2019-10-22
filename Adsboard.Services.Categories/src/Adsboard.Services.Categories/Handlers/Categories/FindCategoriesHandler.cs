using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adsboard.Common.Handlers;
using Adsboard.Services.Categories.Data;
using Adsboard.Services.Categories.Domain;
using Adsboard.Services.Categories.Dto;
using Adsboard.Services.Categories.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Adsboard.Services.Categories.Handlers.Categories
{
    public class FindCategoriesHandler : IQueryHandler<FindCategories, IEnumerable<CategoryDto>>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public FindCategoriesHandler(ApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _categoryRepository = dbContext.Set<Category>();
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> HandleAsync(FindCategories query)
        {
            var categories = await GetCollection(query);
            return categories.Select(x => _mapper.Map<CategoryDto>(x));
        }

        private async Task<IEnumerable<Category>> GetCollection(FindCategories query)
        {
            if (query.Ids != null && query.Ids.Count() > 0)
            {
                return await _categoryRepository.Where(x => query.Ids.Contains(x.Id)).ToListAsync();
            }
            return await _categoryRepository.ToListAsync();
        }
    }
}