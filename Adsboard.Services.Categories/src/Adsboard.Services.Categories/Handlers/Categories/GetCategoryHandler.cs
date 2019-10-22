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
    public class GetCategoryHandler : IQueryHandler<GetCategory, CategoryDto>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryHandler(ApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _categoryRepository = dbContext.Set<Category>();
            _mapper = mapper;
        }

        public async Task<CategoryDto> HandleAsync(GetCategory query)
        {
            var entity = await _categoryRepository.FirstOrDefaultAsync(q => q.Id == query.Id);
            return _mapper.Map<CategoryDto>(entity);
        }
    }
}