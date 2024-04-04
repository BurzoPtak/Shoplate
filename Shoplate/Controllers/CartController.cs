using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoplate.Models;
using Shoplate.Repositories;
using Shoplate.Data;
using Shoplate.Models;
using Shoplate.Repositories;
using System.Text.Json;

namespace ShoplateAPP.Controllers
{
    public class CartController : Controller
    {

        private const string ItemsList = "ItemsList";
        private readonly InterfaceItemRepository _itemrepository;
        private readonly InterfaceOrderRepository _orderRepository;
        private readonly InterfaceUserRepository _userRepository;

        public CartController(InterfaceItemRepository itemrepository, InterfaceOrderRepository orderRepository, InterfaceUserRepository userRepository)
        {
            _itemrepository = itemrepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var sessionItems = HttpContext.Session.GetString(ItemsList);
            var items = string.IsNullOrEmpty(sessionItems)
                ? Enumerable.Empty<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(sessionItems);

            return View(items);
        }

        [HttpPost]
        public IActionResult AddItem(int itemId)
        {
            ViewBag.Powiadomienie = "Aby móc kupować produkty musisz się zarejstrować";
            return View("CreateOrder");
            var item = _itemrepository.GetItem(itemId);

            if (item == null)
                return BadRequest();

            var sessionItems = HttpContext.Session.GetString("ItemsList");
            var items = string.IsNullOrEmpty(sessionItems)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(sessionItems);

            var cartItem = items.FirstOrDefault(i => i.Id == item.Id);

            if (cartItem == null)
            {
                cartItem = new CartItem()
                {
                    Name = item.Name,
                    Price = item.Price,
                    Id = item.Id
                };
                items.Add(cartItem);
            }

            var serializedItems = JsonSerializer.Serialize(items);
            HttpContext.Session.SetString("ItemsList", serializedItems);

            return Ok(cartItem);
        }

        public IActionResult CreateOrder()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateOrder([Bind("City,Address")] Order order)
        {

            if (JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("ItemsList")) == null)
            {
                Console.WriteLine("Pusty kosz");
                return View("Index", order);
            }

            if (HttpContext.Session.GetString("LoggedIn") == "true")
            {
                order.UserId = HttpContext.Session.GetString("ID");
            }
            else
            {
                ViewBag.Powiadomienie = "Aby móc kupować produkty musisz się zarejstrować";
                return View("CreateOrder");
            }

            order.OrderDate = DateTime.Now;
            var items = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("ItemsList"));

            List<Item> orderItems = new List<Item>();

            foreach (var item in items)
            {
                var existingItem = _itemrepository.GetItem(item.Id);
                if (existingItem != null)
                {
                    var orderItem = _itemrepository.GetItem(existingItem.Id);

                    if (orderItem != null)
                    {
                        // Zaktualizuj wartości encji Item na podstawie istniejącego obiektu
                        orderItem.Name = existingItem.Name;
                        orderItem.Description = existingItem.Description;
                        orderItem.Image = existingItem.Image;
                        orderItem.Price = existingItem.Price;
                        orderItem.OrderId = order.Id;

                        orderItems.Add(orderItem);
                    }
                }
            }

            order.Items = orderItems;

            _orderRepository.SaveOrder(order);
            // Wyczyść zawartość koszyka po złożeniu zamówienia
            HttpContext.Session.Remove("ItemsList");

            return View("PlacedOrder", order);
        }


        [HttpPost]
        public IActionResult DeleteItem(int itemId)
        {
            var item = _itemrepository.GetItem(itemId);

            if (item == null)
                return NotFound();

            var sessionItems = HttpContext.Session.GetString(ItemsList);

            if (string.IsNullOrEmpty(sessionItems))
                return BadRequest();

            var items = JsonSerializer.Deserialize<List<CartItem>>(sessionItems);

            var cartItem = items?.FirstOrDefault(i => i.Id == itemId);

            if (cartItem == null)
                return BadRequest();

            items.Remove(cartItem); // Usunięcie elementu z listy

            var serializedItems = JsonSerializer.Serialize(items);
            HttpContext.Session.SetString(ItemsList, serializedItems);

            return Ok(cartItem);
        }

        public IActionResult AllOrders()
        {
            IEnumerable<Order> orders = _orderRepository.GetAll();
            return View(orders);
        }

        public IActionResult AllUserOrders()
        {
            string userid = HttpContext.Session.GetString("ID");
            IEnumerable<Order> orders = _orderRepository.GetAllUserOrders(userid);
            return View(orders);
        }

        public IActionResult OrderDetails(string id)
        {
            Order order = _orderRepository.GetOrder(id);
            User user = _userRepository.GetUserByID(order.UserId);
            var model = new Tuple<Order, User>(order, user);
            return View(model);
        }

        public IActionResult OrderDetailsUser(string id)
        {
            Order order = _orderRepository.GetOrder(id);
            User user = _userRepository.GetUserByID(order.UserId);
            var model = new Tuple<Order, User>(order, user);
            return View(model);
        }

    }
}