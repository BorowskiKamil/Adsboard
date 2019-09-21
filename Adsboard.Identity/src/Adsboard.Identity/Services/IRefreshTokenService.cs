using System;
using System.Threading.Tasks;
using Adsboard.Identity.Dto;

namespace Adsboard.Identity.Services
{
    public interface IRefreshTokenService
    {
        Task AddAsync(Guid userId);
        Task<JsonWebToken> CreateAccessTokenAsync(string refreshToken);
        Task RevokeAsync(string refreshToken, Guid userId);
    }
}