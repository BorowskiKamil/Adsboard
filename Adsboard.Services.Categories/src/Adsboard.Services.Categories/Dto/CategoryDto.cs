using System;

namespace Adsboard.Services.Categories.Dto
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid Creator { get; set; }
    }
}