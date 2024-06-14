using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Subscriptions)
                .WithOne();

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Payments)
                .WithOne(p => p.Client)
                .HasForeignKey(p => p.ClientId);

            modelBuilder.Entity<Subscription>()
                .HasMany(s => s.Payments)
                .WithOne(p => p.Subscription)
                .HasForeignKey(p => p.SubscriptionId);
        }
    }
    
    
}