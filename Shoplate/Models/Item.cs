using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace Shoplate.Models
{
    public class Item
    {
        public Item(int id,string name,string description,string image,decimal price,string? orderId = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Image = image;
            Price = price;
            OrderId = orderId;



        
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string? OrderId { get; set;}
        public Order? Order {  get; set; }



    }
}
