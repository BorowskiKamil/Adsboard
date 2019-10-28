using Adsboard.Services.Users.Domain;
using Adsboard.Services.Users.Dto;
using AutoMapper;

namespace Adsboard.Services.Users.Mappings
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
