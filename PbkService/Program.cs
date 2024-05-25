using static Microsoft.Extensions.Hosting.Host;

namespace PbkService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                .Build()
                .Run();
        }
    }
}
