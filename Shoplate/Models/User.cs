using System.ComponentModel.DataAnnotations;

namespace Shoplate.Models
{
    public class User
    {
        public User(string  id, string name,string surname,string phone,string email,string password,string isAdmin,string username)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Phone = phone;
            Email = email;
            Password = password;
            IsAdmin = isAdmin;
            Username = username;

        }

        [Required]
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }
        public string IsAdmin { get; set; }
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    
}
