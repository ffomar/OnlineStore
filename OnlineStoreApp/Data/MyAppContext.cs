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

        public DbSet<Item> Items { get; set; } = null!;
    }
}