using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStoreApp.Models
{
    public class Item
    {
        [Key]
        [StringLength(24, ErrorMessage = "The Serial Number cannot exceed 24 characters.")]
        public required string Serial {get; set;}

        public required string Name {get; set;}

        public double ?Price {get; set;}

        public string? ClientName {get; set;}
        public string? ClientAddress {get; set;}
        [ForeignKey("ClientName,ClientAddress")]
        public Client? Client {get; set;}

        public int ?CategoryId {get; set;}
        [ForeignKey("CategoryId")]
        public Category ?Category {get;set;}
    }
}