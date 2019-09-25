using System;
using Adsboard.Common.Domain;
using Adsboard.Common.Types;
using Adsboard.Services.Users.Infrastructure;

namespace Adsboard.Services.Users.Domain
{
    public class User : Entity
    {
        public string Email { get; private set; }
        public string FullName { get; private set; }

        private User() { }

        public User(Guid id, string email)
        {
            Id = id;
            Email = email;
        }

        public bool UpdateFullName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AdsboardException(Codes.IncorrectData,
                    $"FullName property can't be empty, entred value: {value}");
            }

            FullName = value;
            return true;
        }
    }
}