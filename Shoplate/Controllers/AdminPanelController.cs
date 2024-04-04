using Microsoft.AspNetCore.Mvc;

namespace Shoplate.Controllers
{
    public class AdminPanelController : Controller
    {
      
        public IActionResult Index()
        {
            return View();
        }
    }
}
