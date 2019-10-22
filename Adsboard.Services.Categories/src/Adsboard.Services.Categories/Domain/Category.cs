using System;
using Adsboard.Common.Domain;
using Adsboard.Common.Types;
using Adsboard.Services.Categories.Infrastructure;

namespace Adsboard.Services.Categories.Domain
{
    public class Category : Entity
    {
        public string Title { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public Guid Creator { get; private set; }

        private Category() { }

        public Category(Guid id, string title, Guid creator)
        {
            if (string.IsNullOrEmpty(title) || title.Length < 5)
            {
                throw new AdsboardException(Codes.WrongCategoryTitle,
                    "Category's title length have to be longer than 4 characters.");
            }

            Id = id;
            Title = title;
            CreatedAt = DateTimeOffset.UtcNow;
            Creator = creator;
        }

        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle) || newTitle.Length < 5)
            {
                throw new AdsboardException(Codes.WrongCategoryTitle,
                    "Category's title length have to be longer than 4 characters.");
            }
            Title = newTitle;
        }
    }
}