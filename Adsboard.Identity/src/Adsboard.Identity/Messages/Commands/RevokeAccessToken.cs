using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Identity.Messages.Commands
{
    public class RevokeAccessToken : ICommand
    {
        public Guid IdentityId { get; }
        public string Token { get; }

        [JsonConstructor]
        public RevokeAccessToken(Guid identityId, string token)
        {
            IdentityId = identityId;
            Token = token;
        }
    }
}