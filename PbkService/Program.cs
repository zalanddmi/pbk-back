using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using PbkService.Data;
using PbkService.Repositories;
using PbkService.Services;
using PbkService.Settings;

namespace PbkService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMvc();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<PbkContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = JwtOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = JwtOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = JwtOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<UserRepository>();

            var app = builder.Build();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            //app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
