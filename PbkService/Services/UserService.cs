using PbkService.Models;
using PbkService.Repositories;
using PbkService.ViewModels;
using System.Security.Cryptography;

namespace PbkService.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        public async Task Create(RegisterViewModel model)
        {
            var(passwordHash, salt) = GeneratePasswordHash(model.Password);
            User user = new()
            {
                Username = model.Username,
                PasswordHash = passwordHash,
                Salt = salt,
                Role = "User"
            };
            await _repository.Create(user);
        }

        private (string, string) GeneratePasswordHash(string password)
        {
            byte[] salt = GenerateSalt();
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 10000);
            return (Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(20)), Convert.ToBase64String(salt));
        }

        private byte[] GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[16];
            rng.GetBytes(salt);
            return salt;
        }
    }
}
