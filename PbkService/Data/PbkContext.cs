using Microsoft.EntityFrameworkCore;

namespace PbkService.Data
{
    public class PbkContext(DbContextOptions<PbkContext> options) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
