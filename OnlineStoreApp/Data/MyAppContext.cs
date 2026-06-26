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

        // Db set for each entity
        public DbSet<Item> Items { get; set; } = null!;

        public DbSet<Category> Categories { set; get;}

        public DbSet<Client> Clients { set; get;}


        // Model relationships and seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Define composite primary key for Client entity
            modelBuilder.Entity<Client>()
                        .HasKey(Client => new { Client.Name, Client.Address });

            // Configure optional many-to-one relationship from Item to Client
            modelBuilder.Entity<Item>()
                .HasOne(x => x.Client)
                .WithMany(x => x.Items)
                .HasForeignKey(i => new { i.ClientName, i.ClientAddress })
                .IsRequired(false);

            // Configure optional many-to-one relationship from Item to Category
            modelBuilder.Entity<Item>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Items)
                .HasForeignKey(i => i.CategoryId)
                .IsRequired(false);


            // Seed 
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" },
                new Category { Id = 3, Name = "Books" }
            );

            Client JoeTheClient = new Client { Name = "John Doe", Address = "123 Main St", Phone = 1234567890 };
            Client JaneTheClient = new Client { Name = "Jane Smith", Address = "456 Elm St", Phone = 9876543210 };
            modelBuilder.Entity<Client>().HasData(
                JoeTheClient,
                JaneTheClient
            );

            /*
            modelBuilder.Entity<Item>().HasData(
                new Item { Serial = "92JR4-6999C-RCJYV-90M24R", Name = "Laptop", Price = 999.99, CategoryId = 1, ClientName = JoeTheClient.Name, ClientAddress = JoeTheClient.Address },
                new Item { Serial = "L28S4-29133-73NYR-JIM343", Name = "T-Shirt", Price = 19.99, CategoryId = 2, ClientName = JaneTheClient.Name, ClientAddress = JaneTheClient.Address }
            );
            */

            base.OnModelCreating(modelBuilder);
        }
    }
}