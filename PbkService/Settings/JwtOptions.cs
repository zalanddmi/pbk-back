using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PbkService.Settings
{
    public class JwtOptions
    {
        const string KEY = "1nonamesecretkey2nonamesecretkey3nonamesecretkey";
        public const string ISSUER = "PbkServer";
        public const string AUDIENCE = "PbkClient";

        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(KEY));
    }
}
