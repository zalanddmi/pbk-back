using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace PbkService.Config
{
    public static class SwaggerConfig
    {
        private static readonly OpenApiSecurityScheme SecurityScheme = new()
        {
            BearerFormat = "JWT",
            Name = "Аутентификация",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Токен",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer().AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(SecurityScheme.Reference.Id, SecurityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { { SecurityScheme, Array.Empty<string>() } });
            });
        }
    }
}