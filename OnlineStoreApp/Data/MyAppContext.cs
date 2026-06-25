using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OnlineStoreApp.Models;

namespace MyApp.Data
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 5, Name = "microphone", Price = 40, SerialNumberId = 15 }
            );
            modelBuilder.Entity<SerialNumber>().HasData(
                new SerialNumber { Id = 15, Name = "mic150", ItemId = 5 }
            );
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<SerialNumber> SerialNumbers { get; set; } = null!;
    }
}