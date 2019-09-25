using System;
using System.Text.RegularExpressions;
using Adsboard.Common.Domain;
using Adsboard.Common.Types;
using Adsboard.Services.Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Adsboard.Services.Identity.Domain
{
    public class Identity : Entity
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public string Email { get; private set; }
        public string Role { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Identity() { }

        public Identity(Guid id, string email)
        {
            if (!EmailRegex.IsMatch(email))
            {
                throw new AdsboardException(Codes.InvalidEmail, 
                    $"Invalid email: '{email}'.");
            }
            Id = id;
            Email = email.ToLowerInvariant();
            Role = "user";
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string password, IPasswordHasher<Identity> passwordHasher)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AdsboardException(Codes.InvalidPassword, 
                    "Password can not be empty.");
            }             
            PasswordHash = passwordHasher.HashPassword(this, password);
        }

        public bool ValidatePassword(string password, IPasswordHasher<Identity> passwordHasher)
            => passwordHasher.VerifyHashedPassword(this, PasswordHash, password) != PasswordVerificationResult.Failed;

    }
}