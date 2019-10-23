using System;
using Adsboard.Common.Domain;

namespace Adsboard.Services.Adverts
{
    public class User : Entity
    {
        public string Email { get; set; }

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