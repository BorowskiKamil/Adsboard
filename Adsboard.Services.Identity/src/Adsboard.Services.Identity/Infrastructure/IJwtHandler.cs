using System.Collections.Generic;
using Adsboard.Services.Identity.Dto;

namespace Adsboard.Services.Identity.Infrastructure
{
    public interface IJwtHandler
    {
        JsonWebToken CreateToken(string userId, string role = null, IDictionary<string, string> claims = null);
        JsonWebTokenPayload GetTokenPayload(string accessToken);
    }
}