using Microsoft.EntityFrameworkCore;
using PbkService.Models;

namespace PbkService.Data
{
    public class PbkContext(DbContextOptions<PbkContext> options) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<User> Users { get; set; }
    }
}
