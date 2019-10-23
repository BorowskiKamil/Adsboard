using System;
using Adsboard.Common.Domain;

namespace Adsboard.Services.Adverts.Domain
{
    public class Category : Entity
    {
        public Guid Creator { get; private set; }

        private Category() 
        {
        }

        public Category(Guid id, Guid creator)
        {
            Id = id;
            Creator = creator;
        }


    }
}