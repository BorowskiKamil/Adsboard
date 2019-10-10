using System;
using Adsboard.Common.Domain;

namespace Adsboard.Services.Adverts.Domain
{
    public class Advert : Entity
    {
        public string Title { get; }
        public string Description { get; }
        public DateTimeOffset CreatedAt { get; }
        public Guid Creator { get; }
        public Guid Category { get; }
        public string Image { get; }
    }
}