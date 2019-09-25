using System;
using Adsboard.Common.Types;
using Adsboard.Services.Users.Dto;

namespace Adsboard.Services.Users.Queries
{
    public class GetUser : IQuery<UserDto>
    {
        public Guid Id { get; set; }
    }
}
