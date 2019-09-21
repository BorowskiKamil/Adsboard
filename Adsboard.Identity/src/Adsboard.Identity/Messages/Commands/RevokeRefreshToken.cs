using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Identity.Messages.Commands
{
    public class RevokeRefreshToken : ICommand
    {
        public Guid IdentityId { get; }
        public string Token { get; }

        [JsonConstructor]
        public RevokeRefreshToken(Guid identityId, string token)
        {
            IdentityId = identityId;
            Token = token;
        }
    }
}