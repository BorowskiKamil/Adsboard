using System;
using Adsboard.Common.Domain;
using Adsboard.Common.Types;
using Adsboard.Services.Adverts.Infrastructure;

namespace Adsboard.Services.Adverts.Domain
{
    public class Advert : Entity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? ArchivedAt { get; private set; }
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


        public void Archive()
        {
            if (ArchivedAt.HasValue)
            {
                throw new AdsboardException(Codes.AdvertAlreadyArchived, 
                    $"This advert's already archived, id: '{Id}'"); 
            }

            ArchivedAt = DateTimeOffset.UtcNow;
        }

        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle) || newTitle.Length < 10)
            {
                throw new AdsboardException(Codes.WrongAdvertTitle,
                    "Advert's title length have to be longer than 9 characters.");
            }
            Title = newTitle;
        }

        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrEmpty(newDescription) || newDescription.Length < 20)
            {
                throw new AdsboardException(Codes.WrongAdvertDescription,
                    "Advert's description length have to be longer than 19 characters.");
            }
            Description = newDescription;
        }

    }
}