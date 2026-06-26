using System.ComponentModel.DataAnnotations;

namespace OnlineStoreApp.Models
{
    public class Client
    {
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public required string Name {get; set;}
        
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public required string Address {get; set;}

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public long ?Phone { get; set; }

        public List<Item>? Items { get; set; }
    }
}