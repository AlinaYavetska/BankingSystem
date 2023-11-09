using Microsoft.EntityFrameworkCore;
using TransactionsService.Models;

namespace TransactionsService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Customer>()
                .HasMany(p => p.Transactions)
                .WithOne(p => p.Customer!)
                .HasForeignKey(p => p.CustomerId);

            modelBuilder
                .Entity<Transaction>()
                .HasOne(p => p.Customer)
                .WithMany(p => p.Transactions)
                .HasForeignKey(p => p.CustomerId);
        }
    }
}
