using Shoplate.Models;

namespace Shoplate.Repositories
{
    public interface InterfaceOrderRepository
    {
        public void SaveOrder(Order order);

        IEnumerable<Order> GetAll();

        Order? GetOrder(string id);

        IEnumerable<Order> GetAllUserOrders(string id);
    }
}