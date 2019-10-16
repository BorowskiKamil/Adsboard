using System;

namespace Adsboard.Services.Adverts.Dto
{
    public class AdvertDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}