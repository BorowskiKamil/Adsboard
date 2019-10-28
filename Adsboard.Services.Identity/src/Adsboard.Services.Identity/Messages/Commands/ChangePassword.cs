using System;
using Adsboard.Common.Messages;
using Newtonsoft.Json;

namespace Adsboard.Services.Identity.Messages.Commands
{
    public class ChangePassword : ICommand
    {
        public Guid IdentityId { get; }
        public string CurrentPassword { get; }
        public string NewPassword { get; }

        [JsonConstructor]
        public ChangePassword(Guid identityId, 
            string currentPassword,string newPassword)
        {
            IdentityId = identityId;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
    }
}