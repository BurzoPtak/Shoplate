using System.ComponentModel.DataAnnotations;

namespace Shoplate.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }

        public ICollection<Item> Items { get; set; }
        public string UserId { get; set; }

        public User User { get; set; }

        public string City { get; set; }
        public string Address { get; set; }



    }
}
