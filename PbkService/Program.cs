using Microsoft.EntityFrameworkCore;
using PbkService.Data;

namespace PbkService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<PbkContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

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
