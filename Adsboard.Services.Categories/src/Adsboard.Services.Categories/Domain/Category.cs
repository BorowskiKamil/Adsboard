using System;
using Adsboard.Common.Domain;
using Adsboard.Common.Types;
using Adsboard.Services.Categories.Infrastructure;

namespace Adsboard.Services.Categories.Domain
{
    public class Category : Entity
    {
        public string Name { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public Guid Creator { get; private set; }

        private Category() { }

        public Category(Guid id, string name, Guid creator)
        {
            if (string.IsNullOrEmpty(name) || name.Length < 5)
            {
                throw new AdsboardException(Codes.WrongCategoryName,
                    "Category's name length have to be longer than 4 characters.");
            }

            Id = id;
            Name = name;
            CreatedAt = DateTimeOffset.UtcNow;
            Creator = creator;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrEmpty(newName) || newName.Length < 5)
            {
                throw new AdsboardException(Codes.WrongCategoryName,
                    "Category's title length have to be longer than 4 characters.");
            }
            Name = newName;
        }
    }
}