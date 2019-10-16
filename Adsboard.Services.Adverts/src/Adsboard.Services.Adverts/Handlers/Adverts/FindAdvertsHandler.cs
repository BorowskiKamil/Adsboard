using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adsboard.Common.Handlers;
using Adsboard.Services.Adverts.Data;
using Adsboard.Services.Adverts.Domain;
using Adsboard.Services.Adverts.Dto;
using Adsboard.Services.Adverts.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Adsboard.Services.Adverts.Handlers.Adverts
{
    public class FindAdvertsHandler : IQueryHandler<FindAdverts, IEnumerable<AdvertDto>>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Advert> _advertRepository;
        private readonly IMapper _mapper;

        public FindAdvertsHandler(ApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _advertRepository = dbContext.Set<Advert>();
            _mapper = mapper;
        }

        public async Task<IEnumerable<AdvertDto>> HandleAsync(FindAdverts query)
        {
            var adverts = await GetCollection(query);
            return adverts.Select(x => _mapper.Map<AdvertDto>(x));
        }

        private async Task<IEnumerable<Advert>> GetCollection(FindAdverts query)
        {
            if (query.UserId.HasValue)
            {
                return await _advertRepository.Where(x => x.Creator == query.UserId).ToListAsync();
            }
            return await _advertRepository.ToListAsync();
        }
    }
}