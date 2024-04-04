using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shoplate.Models;
using Shoplate.Repositories;

namespace Shoplate.Controllers
{
    public class LoginController : Controller
    {
        private readonly InterfaceUserRepository _userRepository;


        public LoginController(InterfaceUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Register(string username, string password, string name, string surname, string phone, string email)
        {

            if (username != null)
            {
                var existingUser = _userRepository.GetUser(username);
                if (existingUser != null)
                {
                    ViewBag.Powiadomienie = "Nazwa jest już zajęta";
                    return View();
                }

                _userRepository.AddUser(Guid.NewGuid().ToString(), name, surname, phone, email, password, "false", username);

                var user = _userRepository.GetUser(username);
                HttpContext.Session.SetString("LoggedIn", "true");
                HttpContext.Session.SetString("Name", user.Name);
                HttpContext.Session.SetString("Surname", user.Surname);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("Phone", user.Phone);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin);
                HttpContext.Session.SetString("ID", user.Id.ToString());

                return View("../Home/Index");
            }
            else return View();

        }

        public IActionResult Login(string username, string password)
        {
            //Zahaszować hasła
            var user = _userRepository.GetUser(username);

            if (user != null && password == user.Password)
            {
                HttpContext.Session.SetString("LoggedIn", "true");
                HttpContext.Session.SetString("Name", user.Name);
                HttpContext.Session.SetString("Surname", user.Surname);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("Phone", user.Phone);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin);
                HttpContext.Session.SetString("ID", user.Id.ToString());
                return View("../Home/Index");
            }
            else
            {
                // ViewBag.Powiadomienie = "Niepoprawna nazwa użytkownika i hasło";
                return View("Login");
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("../Home/Index");
        }

        public IActionResult Account()
        {
            return View();
        }

        public IActionResult ChangePassword(string password, string newPassword1, string newPassword2, string username)
        {
            var user = _userRepository.GetUser(username);
            if (newPassword1 == newPassword2 && user.Password == password)
            {
                _userRepository.ChangePassword(password, newPassword1, newPassword2, username);
                ViewBag.Powiadomienie = "Hasło zostało zmienione";
            }
            else ViewBag.Powiadomienie = "Hasło nie zostało zmienione";

            return View("Account");
        }

        public IActionResult AccountDataChange(string name, string surname, string email, string phone, string password, string currentUsername)
        {
            var user = _userRepository.GetUser(currentUsername);
            if (user.Password == password)
            {

                _userRepository.AccountDataChange(name, surname, email, phone, password, currentUsername);
                HttpContext.Session.SetString("LoggedIn", "true");
                HttpContext.Session.SetString("Name", user.Name);
                HttpContext.Session.SetString("Surname", user.Surname);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("Phone", user.Phone);
                ViewBag.Powiadomienie = "Udało się zmienić dane";
            }
            else
            {
                ViewBag.Powiadomienie = " Nie udało się zmienić dane";
            }
            return View("Account");
        }

    }
}