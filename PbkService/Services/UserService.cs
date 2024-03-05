using Microsoft.IdentityModel.Tokens;
using PbkService.Models;
using PbkService.Repositories;
using PbkService.Settings;
using PbkService.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PbkService.Services
{
    public class UserService(UserRepository repository)
    {
        private readonly UserRepository _repository = repository;

        public async Task Create(RegisterViewModel model)
        {
            var(passwordHash, salt) = GeneratePasswordHash(model.Password);
            User user = new()
            {
                Username = model.Username,
                PasswordHash = passwordHash,
                Salt = salt,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Role = "User"
            };
            await _repository.Create(user);
        }

        public async Task<string> Authenticate(LoginViewModel model)
        {
            User user = _repository.GetByUsername(model.Username);
            if (user != null && VerifyHashedPassword(model.Password, user.PasswordHash, user.Salt))
            {
                string token = GenerateToken(user.Username, user.Role);
                return token;
            }
            return null;
        }

        private (string, string) GeneratePasswordHash(string password)
        {
            byte[] salt = GenerateSalt();
            Rfc2898DeriveBytes rfc2898DeriveBytes = new(password, salt, 10000);
            return (Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(20)), Convert.ToBase64String(salt));
        }

        private byte[] GenerateSalt()
        {
            RNGCryptoServiceProvider rng = new();
            byte[] salt = new byte[16];
            rng.GetBytes(salt);
            return salt;
        }

        private bool VerifyHashedPassword(string password, string passwordHash, string salt)
        {
            byte[] hashBytes = Convert.FromBase64String(passwordHash);
            byte[] saltBytes = Convert.FromBase64String(salt);
            using Rfc2898DeriveBytes rfc2898DeriveBytes = new(password, saltBytes, 10000);
            byte[] generatedHashBytes = rfc2898DeriveBytes.GetBytes(20);
            return hashBytes.SequenceEqual(generatedHashBytes);
        }

        private string GenerateToken(string username, string role)
        {
            List<Claim> claims =
            [
                new(ClaimTypes.Name, username),
                new(ClaimTypes.Role, role)
            ];
            JwtSecurityToken token = new
                (
                    issuer: JwtOptions.ISSUER,
                    audience: JwtOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
