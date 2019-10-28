using System;
using System.Threading.Tasks;
using Adsboard.Common.RabbitMq;
using Adsboard.Common.Types;
using Adsboard.Services.Identity.Data;
using Adsboard.Services.Identity.Domain;
using Adsboard.Services.Identity.Dto;
using Adsboard.Services.Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Adsboard.Services.Identity.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<RefreshToken> _refreshTokenRepository;
        private readonly DbSet<Domain.Identity> _identityRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IPasswordHasher<Domain.Identity> _passwordHasher;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IBusPublisher _busPublisher;

        public RefreshTokenService(ApplicationContext dbContext,
            IJwtHandler jwtHandler,
            IPasswordHasher<Domain.Identity> passwordHasher,
            IClaimsProvider claimsProvider,
            IBusPublisher busPublisher)
        {
            _dbContext = dbContext;
            _refreshTokenRepository = _dbContext.Set<RefreshToken>();
            _identityRepository = _dbContext.Set<Domain.Identity>();
            _jwtHandler = jwtHandler;
            _passwordHasher = passwordHasher;
            _claimsProvider = claimsProvider;
            _busPublisher = busPublisher;
        }

        public async Task AddAsync(Guid identityId)
        {
            var identity = await _identityRepository.FirstOrDefaultAsync(x => x.Id == identityId);
            if (identity == null)
            {
                throw new AdsboardException(Codes.IdentityNotFound, 
                    $"identity: '{identityId}' was not found.");
            }
            await _refreshTokenRepository.AddAsync(new RefreshToken(identity, _passwordHasher));

            var savingResult = await _dbContext.SaveChangesAsync();
            if (savingResult == 0) 
            {
                throw new AdsboardException(Codes.SavingError,
                    $"An error occured during saving data.");
            }            
        }

        public async Task<JsonWebToken> CreateAccessTokenAsync(string token)
        {
            var refreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(x => x.Token == token);
            if (refreshToken == null)
            {
                throw new AdsboardException(Codes.RefreshTokenNotFound, 
                    "Refresh token was not found.");
            }
            if (refreshToken.Revoked)
            {
                throw new AdsboardException(Codes.RefreshTokenAlreadyRevoked, 
                    $"Refresh token: '{refreshToken.Id}' was revoked.");
            }
            var identity = await _identityRepository.FirstOrDefaultAsync(x => x.Id == refreshToken.IdentityId);
            if (identity == null)
            {
                throw new AdsboardException(Codes.IdentityNotFound, 
                    $"identity: '{refreshToken.IdentityId}' was not found.");
            }
            var claims = await _claimsProvider.GetAsync(identity.Id);
            var jwt = _jwtHandler.CreateToken(identity.Id.ToString("N"), identity.Role, claims);
            jwt.RefreshToken = refreshToken.Token;
            // await _busPublisher.PublishAsync(new AccessTokenRefreshed(identity.Id), CorrelationContext.Empty);
            
            return jwt;
        }

        public async Task RevokeAsync(string token, Guid identityId)
        {
            var refreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(x => x.Token == token && x.IdentityId == identityId);
            if (refreshToken == null)
            {
                throw new AdsboardException(Codes.RefreshTokenNotFound, 
                    "Refresh token was not found.");
            }
            refreshToken.Revoke();
            _refreshTokenRepository.Update(refreshToken);

            var savingResult = await _dbContext.SaveChangesAsync();
            if (savingResult == 0) 
            {
                throw new AdsboardException(Codes.SavingError,
                    $"An error occured during saving data.");
            }
            // await _busPublisher.PublishAsync(new RefreshTokenRevoked(refreshToken.identityId), CorrelationContext.Empty);
        }
    }
}