using System;
using System.Collections.Generic;
using System.Linq;

namespace Adsboard.Services.Categories.Queries
{
    public class FindCategories
    {
        public IQueryable<Guid> Ids { get; set; }
    }
}