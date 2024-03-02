using Microsoft.EntityFrameworkCore;
using PbkService.Data;
using PbkService.Repositories;
using PbkService.Services;

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
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<UserRepository>();

            var app = builder.Build();
            app.UseRouting();
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
