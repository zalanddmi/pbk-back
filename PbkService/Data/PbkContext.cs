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
        public DbSet<Mcc> MCCs { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Cashback> Cashbacks { get; set; }
        public DbSet<MccPbkCategory> MccPbkCategories { get; set; }
        public DbSet<Outlet> Outlets { get; set; }
        public DbSet<PbkCategory> PbkCategories { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<TypeCard> TypeCards { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<UserCard> UserCards { get; set; }
    }
}
