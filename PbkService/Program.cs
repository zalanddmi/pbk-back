using static Microsoft.Extensions.Hosting.Host;

namespace PbkService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.UseSystemd();
                })
                .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                .Build()
                .Run();
        }
    }
}
