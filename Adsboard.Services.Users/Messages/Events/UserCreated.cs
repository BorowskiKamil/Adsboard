using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Services.Users.Messages.Events
{
    public class UserCreated : IEvent
    {
        public Guid Id { get; }
        public string Email { get; }

        [JsonConstructor]
        public UserCreated(Guid id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}