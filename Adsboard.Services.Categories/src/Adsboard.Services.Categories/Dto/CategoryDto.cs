using System;

namespace Adsboard.Services.Categories.Dto
{
    public class CategoryDto
    {
        public string Title { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid Creator { get; set; }
    }
}