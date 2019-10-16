using Adsboard.Services.Adverts.Domain;
using Adsboard.Services.Adverts.Dto;
using AutoMapper;

namespace Adsboard.Services.Adverts.Mappings
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            CreateMap<Advert, AdvertDto>();
        }
    }
}
