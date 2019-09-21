using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Identity.Messages.Events
{
    public class IdentityCreated : IEvent
    {
        public Guid IdentityId { get; }
        public string Email { get; }
        public string Role { get; }

        [JsonConstructor]
        public IdentityCreated(Guid identityId, string email, string role)
        {
            IdentityId = identityId;
            Email = email;
            Role = role;
        }
    }
}