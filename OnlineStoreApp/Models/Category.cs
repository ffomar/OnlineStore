namespace OnlineStoreApp.Models
{
    public class Category
    {
        public int id { get; set; }
        public string Name { get; set; } = null!;
        public List<Item>? Items { get; set; }

    }
}