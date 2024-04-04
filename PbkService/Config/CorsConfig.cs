using Microsoft.AspNetCore.Cors.Infrastructure;

namespace PbkService.Config
{
    public static class CorsConfig
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("Development", AllowAllPolicy));
        }

        private static void AllowAllPolicy(CorsPolicyBuilder builder)
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    }
}