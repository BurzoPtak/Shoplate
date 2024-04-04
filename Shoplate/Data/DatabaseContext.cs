using Microsoft.EntityFrameworkCore;
using Shoplate.Models;

namespace Shoplate.Data
{
    //Connecting using Entity Framework
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasOne<Order>(s => s.Order)
                .WithMany(g => g.Items)
                .HasForeignKey(s => s.OrderId);
            modelBuilder.Entity<Order>()
                .HasOne<User>(s => s.User)
                .WithMany(g => g.Orders)
                .HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User>().HasData(new User(Guid.NewGuid().ToString(),"John","Doe","000000000","temp@mail.com","password","true","admin"));
            modelBuilder.Entity<Item>().HasData(new Item(1, "Smiley", "Very smiley face :)", "Smile.png", 9.99M));

        }
    }
}
