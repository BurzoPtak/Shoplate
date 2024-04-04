using Microsoft.EntityFrameworkCore;
using Shoplate.Data;
using Shoplate.Models;


namespace Shoplate.Repositories
{
    public class OrderRepository : InterfaceOrderRepository
    {
        private DatabaseContext context;

        public OrderRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public void SaveOrder(Order order)
        {
            context.Orders.Add(order);
            context.SaveChanges();
        }

        public IEnumerable<Order> GetAll()
        {
            return context.Orders.ToList();
        }

        public Order? GetOrder(string id)
        {
            var order = context.Orders
                .Include(o => o.Items)
                .Include(o => o.User)
                .FirstOrDefault(o => o.Id == id);

            return order;
        }

        public IEnumerable<Order> GetAllUserOrders(string id)
        {
            return context.Orders.Where(items => items.UserId == id).ToList();
        }

    }
}