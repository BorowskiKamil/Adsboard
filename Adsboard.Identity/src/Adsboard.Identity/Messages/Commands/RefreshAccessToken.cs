using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Identity.Messages.Commands
{
    public class RefreshAccessToken : ICommand
    {
        public string Token { get; }

        [JsonConstructor]
        public RefreshAccessToken(string token)
        {
            Token = token;
        }
    }
}
