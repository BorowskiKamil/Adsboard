using Adsboard.Services.Categories.Domain;
using Adsboard.Services.Categories.Dto;
using AutoMapper;

namespace Adsboard.Services.Categories.Mappings
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
