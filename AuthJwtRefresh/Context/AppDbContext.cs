using AuthJwtRefresh.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthJwtRefresh.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany<RefreshToken>(a => a.RefreshTokens)
                .WithOne(r => r.Account)
                .HasForeignKey(r => r.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Account>().HasData(
                new Account[]
                {
                    new Account{Id = new Guid("c68b01a0-3bc6-4c84-a2f9-3aa04768cf80"), Email = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"), Role = "Admin" }
                });
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
