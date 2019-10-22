using System;
using System.Collections.Generic;

namespace Adsboard.Services.Categories.Queries
{
    public class FindCategories
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}