using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PbkService.Settings;

namespace PbkService.Config
{
    public static class AuthenticationConfig
    {
        private static readonly TokenValidationParameters ValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = JwtOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = JwtOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = JwtOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };

        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(DefaultJwtOptions);
        }

        private static void DefaultJwtOptions(JwtBearerOptions options)
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = ValidationParameters;
        }
    }
}