﻿using Microsoft.EntityFrameworkCore;
using PbkService.Config;
using PbkService.Data;
using PbkService.Repositories;
using PbkService.Services;

namespace PbkService
{
    public class Startup
    {
        private IConfigurationRoot Configuration { get; } = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureAuthentication();
            services.ConfigureCors();
            services.ConfigureSwagger();

            services.AddControllers();
            services.AddAuthentication();

            services.AddDbContext<PbkContext>(options =>
            {
                options.UseLazyLoadingProxies().UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<AccountService>();
            services.AddScoped<UserRepository>();
            services.AddScoped<MccService>();
            services.AddScoped<MccRepository>();
            services.AddScoped<BankRepository>();
            services.AddScoped<BankService>();
            services.AddScoped<TypeCardRepository>();
            services.AddScoped<TypeCardService>();
            services.AddScoped<ShopRepository>();
            services.AddScoped<ShopService>();
            services.AddScoped<OutletRepository>();
            services.AddScoped<OutletService>();
            services.AddScoped<PbkCategoryRepository>();
            services.AddScoped<PbkCategoryService>();
            services.AddScoped<MccPbkCategoryRepository>();
            services.AddScoped<CashbackRepository>();
            services.AddScoped<CardRepository>();
            services.AddScoped<CardService>();
            services.AddScoped<OperationService>();
            services.AddScoped<OperationRepository>();
            services.AddScoped<AlgorithmService>();
            services.AddScoped<UserCardService>();
            services.AddScoped<UserCardRepository>();
        }


        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("Development");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}