using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Adsboard.Services.Identity.Infrastructure;
using Adsboard.Services.Identity.Services;
using Adsboard.Services.Identity.Messages.Commands;
using Adsboard.Common.Mvc;

namespace Adsboard.Services.Identity.Controllers
{
    [Route("")]
    [ApiController]
    [JwtAuth]
    public class TokensController : BaseController
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public TokensController(IAccessTokenService accessTokenService,
            IRefreshTokenService refreshTokenService)
        {
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
        }
        
        [HttpPost("access-tokens/refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshAccessToken command)
            => Ok(await _refreshTokenService.CreateAccessTokenAsync(command.Token));

        [HttpPost("access-tokens/revoke")]
        public async Task<IActionResult> RevokeAccessToken(RevokeAccessToken command)
        {
            await _accessTokenService.DeactivateCurrentAsync(
                command.Bind(c => c.IdentityId, UserId).IdentityId.ToString("N"));

            return NoContent();
        }

        [HttpPost("refresh-tokens/revoke")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeRefreshToken command)
        {
            await _refreshTokenService.RevokeAsync(command.Token, 
                command.Bind(c => c.IdentityId, UserId).IdentityId);

            return NoContent();
        }
    }
}