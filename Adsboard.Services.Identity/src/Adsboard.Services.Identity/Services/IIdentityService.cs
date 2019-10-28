using System;
using System.Threading.Tasks;
using Adsboard.Services.Identity.Dto;

namespace Adsboard.Services.Identity.Services
{
    public interface IIdentityService
    {
        Task<JsonWebToken> SignUpAsync(Guid id, string email, string password, string role = "user");
        Task<JsonWebToken> SignInAsync(string email, string password);
        Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);         
    }
}