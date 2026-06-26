using System.ComponentModel.DataAnnotations;

namespace OnlineStoreApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }

        public List<Item>? Items { get; set; }
    }
}