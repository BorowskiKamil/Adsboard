using System;
using System.Threading.Tasks;
using Adsboard.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Adsboard.Identity.Domain;
using Identity = Adsboard.Identity.Domain;
using Adsboard.Identity.Dto;
using Adsboard.Common.Types;
using Adsboard.Identity.Infrastructure;
using Adsboard.Common.RabbitMq;
using Adsboard.Identity.Messages.Events;

namespace Adsboard.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly ApplicationContext _dbContext;
        private readonly DbSet<Domain.Identity> _identityRepository;
        private readonly IPasswordHasher<Domain.Identity> _passwordHasher;
        private readonly IJwtHandler _jwtHandler;
        private readonly DbSet<RefreshToken> _refreshTokenRepository;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IBusPublisher _busPublisher;

        public IdentityService(ApplicationContext dbContext,
            IPasswordHasher<Domain.Identity> passwordHasher,
            IJwtHandler jwtHandler,
            IClaimsProvider claimsProvider,
            IBusPublisher busPublisher)
        {
            _dbContext = dbContext;
            _identityRepository = _dbContext.Set<Domain.Identity>();
            _passwordHasher = passwordHasher;
            _jwtHandler = jwtHandler;
            _refreshTokenRepository = _dbContext.Set<RefreshToken>();
            _claimsProvider = claimsProvider;
            _busPublisher = busPublisher;
        }

        public async Task<JsonWebToken> SignUpAsync(Guid id, string email, string password, string role = "identity")
        {
            var identity = await _identityRepository.FirstOrDefaultAsync(x => x.Email == email);
            if (identity != null)
            {
                throw new AdsboardException(Codes.EmailInUse,
                    $"Given email '{email}' is already in use.");
            }
            if (string.IsNullOrWhiteSpace(role))
            {
                role = "identity";
            }
            identity = new Domain.Identity(id, email);
            identity.SetPassword(password, _passwordHasher);

            await _identityRepository.AddAsync(identity);

            var savingResult = await _dbContext.SaveChangesAsync();
            if (savingResult == 0) 
            {
                throw new AdsboardException(Codes.SavingError,
                    $"An error occured during saving data.");
            }
            await _busPublisher.PublishAsync(new IdentityCreated(id, email, role), CorrelationContext.Empty);

            return await SignInAsync(identity.Email, password);
        }

        public async Task<JsonWebToken> SignInAsync(string email, string password)
        {
            var identity = await _identityRepository.FirstOrDefaultAsync(x => x.Email == email);
            if (identity == null || !identity.ValidatePassword(password, _passwordHasher))
            {
                throw new AdsboardException(Codes.InvalidCredentials,
                    "Invalid credentials.");
            }
            var refreshToken = new RefreshToken(identity, _passwordHasher);
            var claims = await _claimsProvider.GetAsync(identity.Id);
            var jwt = _jwtHandler.CreateToken(identity.Id.ToString("N"), identity.Role, claims);
            jwt.RefreshToken = refreshToken.Token;
            jwt.Email = identity.Email;

            await _refreshTokenRepository.AddAsync(refreshToken);

            var savingResult = await _dbContext.SaveChangesAsync();
            if (savingResult == 0) 
            {
                throw new AdsboardException(Codes.SavingError,
                    $"An error occured during saving data.");
            }

            return jwt;
        }

        public async Task ChangePasswordAsync(Guid identityId, string currentPassword, string newPassword)
        {
            var identity = await _identityRepository.FirstOrDefaultAsync(x => x.Id == identityId);
            if (identity == null)
            {
                throw new AdsboardException(Codes.IdentityNotFound, 
                    $"Identity with id: '{identityId}' was not found.");
            }
            if (!identity.ValidatePassword(currentPassword, _passwordHasher))
            {
                throw new AdsboardException(Codes.InvalidCurrentPassword, 
                    "Invalid current password.");
            }
            identity.SetPassword(newPassword, _passwordHasher);
            _identityRepository.Update(identity);

            var savingResult = await _dbContext.SaveChangesAsync();
            if (savingResult == 0) 
            {
                throw new AdsboardException(Codes.SavingError,
                    $"An error occured during saving data.");
            }
            // await _busPublisher.PublishAsync(new PasswordChanged(identityId), CorrelationContext.Empty);         
        }
    }
}