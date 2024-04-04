using Shoplate.Models;
namespace Shoplate.Repositories
{
    public interface InterfaceItemRepository
    {
        IEnumerable<Item> GetAll();

        Item? GetItem(int id);

        IEnumerable<Item> GetEveryUnordered();

        IEnumerable<Item> SearchItemByName(string name);

        void AddItem(int id, string name, string description, string image, decimal price);

        void DeleteItem(int id);

        public void ChangeItemData(int id, string name, string description, string image, decimal price);

    }
}