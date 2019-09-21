using System.Collections.Generic;
using Adsboard.Identity.Dto;

namespace Adsboard.Identity.Infrastructure
{
    public interface IJwtHandler
    {
        JsonWebToken CreateToken(string userId, string role = null, IDictionary<string, string> claims = null);
        JsonWebTokenPayload GetTokenPayload(string accessToken);
    }
}