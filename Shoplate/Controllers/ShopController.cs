using Microsoft.AspNetCore.Mvc;
using Shoplate.Models;
using Shoplate.Repositories;
using Shoplate.Models;
using Shoplate.Repositories;
using System.Xml.Linq;

namespace ShoplateAPP.Controllers
{
    public class ShopController : Controller
    {
        private readonly InterfaceItemRepository _itemrepository;
        private readonly InterfaceUserRepository _userrepository;

        public ShopController(InterfaceItemRepository iitemRepository, InterfaceUserRepository iuserrepository)
        {
            _itemrepository = iitemRepository;
            _userrepository = iuserrepository;
        }

        public IActionResult Index(string sortOrder, string minPrice, string maxPrice, string name)
        {
            IEnumerable<Item> items = _itemrepository.GetEveryUnordered();

            if (!string.IsNullOrEmpty(name))
            {
                items = items.Where(item => item.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(minPrice) && decimal.TryParse(minPrice, out decimal minPriceValue))
            {
                items = items.Where(item => item.Price >= minPriceValue);
            }

            if (!string.IsNullOrEmpty(maxPrice) && decimal.TryParse(maxPrice, out decimal maxPriceValue))
            {
                items = items.Where(item => item.Price <= maxPriceValue);
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "ascending")
                {
                    items = items.OrderBy(item => item.Price);
                }
                else if (sortOrder == "descending")
                {
                    items = items.OrderByDescending(item => item.Price);
                }
            }

            return View(items);
        }

        public IActionResult ItemDetails(int id)
        {
            Item? item = _itemrepository.GetItem(id);
            return View(item);
        }

    }
}