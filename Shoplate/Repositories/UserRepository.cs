using Shoplate.Data;
using Shoplate.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Shoplate.Repositories
{
    public class UserRepository : InterfaceUserRepository
    {
        private DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void AddUser(string id, string name, string surname, string phone, string email, string password, string isAdmin, string username)
        {
            User NewUser = new User(id, name, surname, phone, email, password, isAdmin, username);
            _context.Users.Add(NewUser);
            _context.SaveChanges();
        }

        public void ChangePassword(string password, string newPassword1, string newPassword2, string username)
        {

            var Username = _context.Users.FirstOrDefault(u => u.Username == username);
            Username.Password = newPassword2;
            _context.SaveChanges();
        }

        public void AccountDataChange(string name, string surname, string email, string phone, string password, string currentUsername)
        {

            var Username = _context.Users.FirstOrDefault(u => u.Username == currentUsername);
            Username.Name = name;
            Username.Surname = surname;
            Username.Email = email;
            Username.Phone = phone;
            _context.SaveChanges();


        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetUser(string username)
        {
            return _context.Users.FirstOrDefault(user => user.Username == username);
        }
        public User GetUserByID(string id)
        {
            return _context.Users.FirstOrDefault(user => user.Id == id);
        }

    }
}