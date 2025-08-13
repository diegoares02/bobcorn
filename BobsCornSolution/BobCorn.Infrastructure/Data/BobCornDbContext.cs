using BobsCorn.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BobsCorn.Infrastructure.Data
{
    public class BobCornDbContext : DbContext
    {
        public BobCornDbContext(DbContextOptions<BobCornDbContext> options)
               : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserProductLog> UserProductLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProductLog>()
                .HasOne(log => log.User)
                .WithMany(user => user.UserProductLogs)
                .HasForeignKey(log => log.UserId);

            modelBuilder.Entity<UserProductLog>()
                .HasOne(log => log.Product)
                .WithMany(product => product.UserProductLogs)
                .HasForeignKey(log => log.ProductId);
        }
    }
}
