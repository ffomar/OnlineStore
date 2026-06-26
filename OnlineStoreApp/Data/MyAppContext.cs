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
            modelBuilder.Entity<ItemClient>().HasKey(ic => new { 
                ic.itemId,
                ic.ClientId
            });

            modelBuilder.Entity<ItemClient>().HasOne(i=>i.Item).WithMany(ic => ic.ItemClients).HasForeignKey(i=>i.itemId);
            modelBuilder.Entity<ItemClient>().HasOne(ic=>ic.Client).WithMany(c => c.ItemClients).HasForeignKey(ic=>ic.ClientId);

            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 5, Name = "microphone", Price = 40, SerialNumberId = 15 }
            );
            modelBuilder.Entity<SerialNumber>().HasData(
                new SerialNumber { Id = 1, Name = "mic150", ItemId = 5 }
            );
            modelBuilder.Entity<Category>().HasData(
                new Category{ id = 1, Name = "Electronics"},
                new Category{ id = 2, Name = "Books"}
            );
            modelBuilder.Entity<Client>().HasData(
                new Client{ id = 1, Name = "Zac"},
                new Client{ id = 2, Name = "Martin"}
            );
            modelBuilder.Entity<ItemClient>().HasData(
                new ItemClient{ itemId = 5, ClientId = 1},
                new ItemClient{ itemId = 4, ClientId = 2}
            );
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<SerialNumber> SerialNumbers { get; set; } = null!;
        public DbSet<Category> Categories { set; get;}
        public DbSet<Client> Clients { set; get;}
        public DbSet<ItemClient> ItemClients { set; get;}
    }
}