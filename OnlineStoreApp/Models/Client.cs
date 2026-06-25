namespace OnlineStoreApp.Models
{
    public class Client
    {
        public int id { get; set; }
        public string Name { get; set; } = null!;
        public List<Item>? Items { get; set; }
        public List<ItemClient>? ItemClients { get; set;}
    }
}