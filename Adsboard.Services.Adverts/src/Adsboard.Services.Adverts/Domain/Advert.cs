using System;
using Adsboard.Common.Domain;

namespace Adsboard.Services.Adverts.Domain
{
    public class Advert : Entity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public Guid Creator { get; private set; }
        public Guid Category { get; private set; }
        public string Image { get; private set; }

        private Advert() { }

        public Advert(Guid id, string title, string description, Guid creator, Guid category, string image = null)
        {
            Id = id;
            Title = title;
            Description = description;
            CreatedAt = DateTimeOffset.UtcNow;
            Creator = creator;
            Category = category;
            Image = image;
        }
    }
}