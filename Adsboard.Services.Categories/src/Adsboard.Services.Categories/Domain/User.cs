using System;
using Adsboard.Common.Domain;

namespace Adsboard.Services.Categories.Domain
{
    public class User : Entity
    {
        public string Email { get; private set; }

        private User() 
        {
        }

        public User(Guid id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}