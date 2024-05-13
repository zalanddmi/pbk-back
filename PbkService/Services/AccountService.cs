using Microsoft.IdentityModel.Tokens;
using PbkService.Auxiliaries.Exceptions.User;
using PbkService.Models;
using PbkService.Repositories;
using PbkService.Requests;
using PbkService.Settings;
using PbkService.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PbkService.Services
{
    public class AccountService(UserRepository userRepository)
    {
        private readonly UserRepository _userRepository = userRepository;

        private const string RegexEmail = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9-]+\.[A-Za-z]{2,4}$";
        private const string RegexPhoneNumber = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";

        public async Task<UserDTO> Register(RegisterRequest request)
        {
            User? userByUsername = _userRepository.GetByUsername(request.Username);
            if (userByUsername != null)
            {
                throw new UserUsernameExists($"Пользователь с ником {request.Username} существует.");
            }
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                if (!Regex.IsMatch(request.Email, RegexEmail))
                {
                    throw new InvalidUserEmail($"Некорретное значение почты {request.Email}.");
                }
                User? userByEmail = await _userRepository.GetByEmail(request.Email);
                if (userByEmail != null)
                {
                    throw new UserEmailExists($"Пользователь с почтой {request.Email} существует.");
                }
            }
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                if (!Regex.IsMatch(request.PhoneNumber, RegexPhoneNumber))
                {
                    throw new InvalidUserPhonenumber($"Некорретное значение номера {request.PhoneNumber}.");
                }
                User? userByPhoneNumber = await _userRepository.GetByPhoneNumber(request.PhoneNumber);
                if (userByPhoneNumber != null)
                {
                    throw new UserPhonenumberExists($"Пользователь с номером {request.PhoneNumber} существует.");
                }
            }
            var (passwordHash, salt) = GeneratePasswordHash(request.Password);
            User user = new()
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Salt = salt,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Role = "User"
            };
            await _userRepository.Create(user);
            string token = GenerateToken(user.Username, user.Role);
            UserDTO userDTO = new()
            {
                Username = user.Username,
                Token = token
            };
            return userDTO;
        }

        public async Task<UserDTO> Login(LoginRequest request)
        {
            User? user;
            if (Regex.IsMatch(request.Login, RegexEmail))
            {
                user = await _userRepository.GetByEmail(request.Login);
                if (user == null)
                {
                    throw new UserEmailNotExists($"Пользователя с почтой {request.Login} не существует.");
                }
            }
            else if (Regex.IsMatch(request.Login, RegexPhoneNumber))
            {
                user = await _userRepository.GetByPhoneNumber(request.Login);
                if (user == null)
                {
                    throw new UserPhonenumberNotExists($"Пользователя с номером {request.Login} не существует.");
                }
            }
            else
            {
                user = _userRepository.GetByUsername(request.Login);
                if (user == null)
                {
                    throw new UserUsernameNotExists($"Пользователя с ником {request.Login} не существует.");
                }
            }

            if (!VerifyHashedPassword(request.Password, user.PasswordHash, user.Salt))
            {
                throw new InvalidUserPassword("Введенный пароль неверен.");
            }
            string token = GenerateToken(user.Username, user.Role);
            UserDTO userDTO = new()
            {
                Username = user.Username,
                Token = token
            };
            return userDTO;
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
