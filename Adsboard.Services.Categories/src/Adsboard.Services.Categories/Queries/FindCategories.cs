using System;
using System.Collections.Generic;
using System.Linq;
using Adsboard.Common.Types;
using Adsboard.Services.Categories.Dto;

namespace Adsboard.Services.Categories.Queries
{
    public class FindCategories : IQuery<IEnumerable<CategoryDto>>
    {
        public IQueryable<Guid> Ids { get; set; }
    }
}