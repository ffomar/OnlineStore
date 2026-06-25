namespace OnlineStoreApp.Models
{
    public class ItemClient
    {
        public int itemId { get; set; }
        public Item Item { get; set; } = null!;
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

    }
}