using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Identity.Messages.Commands
{
    public class SignIn : ICommand
    {
        public string Email { get; }
        public string Password { get; }

        [JsonConstructor]
        public SignIn(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}