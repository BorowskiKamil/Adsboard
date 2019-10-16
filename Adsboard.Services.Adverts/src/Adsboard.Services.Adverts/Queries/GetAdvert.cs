using System;
using Adsboard.Common.Types;
using Adsboard.Services.Adverts.Dto;

namespace Adsboard.Services.Adverts.Queries
{
    public class GetAdvert : IQuery<AdvertDto>
    {
        public Guid Id { get; set; }
    }
}
