using System.Threading.Tasks;
using Adsboard.Services.Identity.Dto;

namespace Adsboard.Services.Identity.Infrastructure
{
    public interface IAccessTokenService
    {
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync(string userId);
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string userId, string token);
    }
}