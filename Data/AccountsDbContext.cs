using BigPurpleBank.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BigPurpleBank.Data
{
    public class AccountsDbContext : DbContext
    {
        public AccountsDbContext(DbContextOptions<AccountsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .Property(a => a.ProductCategory)
                .HasConversion<string>();

            modelBuilder.Entity<Account>()
                .Property(a => a.OpenStatus)
                .HasConversion<string>();
        }
    }
}
