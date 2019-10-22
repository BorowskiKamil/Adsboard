using System;
using Adsboard.Common.Types;
using Adsboard.Services.Categories.Dto;

namespace Adsboard.Services.Categories.Queries
{
    public class GetCategory : IQuery<CategoryDto>
    {
        public Guid Id { get; set; }
    }
}
