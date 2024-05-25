using Microsoft.EntityFrameworkCore;
using PbkService.Config;
using PbkService.Data;
using PbkService.Repositories;
using PbkService.Services;

namespace PbkService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureCors();
            builder.Services.ConfigureAuthentication();
            builder.Services.ConfigureSwagger();

            builder.Services.AddControllers();
            builder.Services.AddAuthentication();

            builder.Services.AddDbContext<PbkContext>(options =>
            {
                options.UseLazyLoadingProxies().UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<MccService>();
            builder.Services.AddScoped<MccRepository>();
            builder.Services.AddScoped<BankRepository>();
            builder.Services.AddScoped<BankService>();
            builder.Services.AddScoped<TypeCardRepository>();
            builder.Services.AddScoped<TypeCardService>();
            builder.Services.AddScoped<ShopRepository>();
            builder.Services.AddScoped<ShopService>();
            builder.Services.AddScoped<OutletRepository>();
            builder.Services.AddScoped<OutletService>();
            builder.Services.AddScoped<PbkCategoryRepository>();
            builder.Services.AddScoped<PbkCategoryService>();
            builder.Services.AddScoped<MccPbkCategoryRepository>();
            builder.Services.AddScoped<CashbackRepository>();
            builder.Services.AddScoped<CardRepository>();
            builder.Services.AddScoped<CardService>();
            builder.Services.AddScoped<OperationService>();
            builder.Services.AddScoped<OperationRepository>();
            builder.Services.AddScoped<AlgorithmService>();
            builder.Services.AddScoped<UserCardService>();
            builder.Services.AddScoped<UserCardRepository>();

            var app = builder.Build();
            app.UseStaticFiles();
            app.UseCors("Development");
            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapFallbackToFile("index.html");
            app.Run();
        }
    }
}
