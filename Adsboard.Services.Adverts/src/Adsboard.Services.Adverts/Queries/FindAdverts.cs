using System;
using System.Collections.Generic;
using System.Linq;
using Adsboard.Common.Types;
using Adsboard.Services.Adverts.Dto;

namespace Adsboard.Services.Adverts.Queries
{
    public class FindAdverts : IQuery<IEnumerable<AdvertDto>>
    {
        public Guid? UserId { get; set; }
    }
}