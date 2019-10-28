using System;
using Adsboard.Common.Domain;
using Adsboard.Common.Types;
using Adsboard.Services.Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Adsboard.Services.Identity.Domain
{
    public class RefreshToken : Entity
    {
        public Guid IdentityId { get; private set; }
        public string Token { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public bool Revoked => RevokedAt.HasValue;

        private RefreshToken() {}

        public RefreshToken(Identity identity, IPasswordHasher<Identity> passwordHasher)
        {
            Id = Guid.NewGuid();
            IdentityId = identity.Id;
            CreatedAt = DateTime.UtcNow;
            Token = CreateToken(identity, passwordHasher);
        }

        public void Revoke()
        {
            if (Revoked)
            {
                throw new AdsboardException(Codes.RefreshTokenAlreadyRevoked, 
                    $"Refresh token: '{Id}' has been revoked already at '{RevokedAt}'.");
            }
            RevokedAt = DateTime.UtcNow;
        }

        private static string CreateToken(Identity identity, IPasswordHasher<Identity> passwordHasher)
            => passwordHasher.HashPassword(identity, Guid.NewGuid().ToString("N"))
                .Replace("=", string.Empty)
                .Replace("+", string.Empty)
                .Replace("/", string.Empty);
    }
}