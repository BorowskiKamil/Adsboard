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
    public class GetAdvertHandler : IQueryHandler<GetAdvert, AdvertDto>
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Advert> _advertRepository;
        private readonly IMapper _mapper;

        public GetAdvertHandler(ApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _advertRepository = dbContext.Set<Advert>();
            _mapper = mapper;
        }

        public async Task<AdvertDto> HandleAsync(GetAdvert query)
        {
            var entity = await _advertRepository.FirstOrDefaultAsync(q => q.Id == query.Id);
            return _mapper.Map<AdvertDto>(entity);
        }
    }
}