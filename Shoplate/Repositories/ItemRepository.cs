using Shoplate.Data;
using Shoplate.Models;


namespace Shoplate.Repositories
{
    public class ItemRepository : InterfaceItemRepository
    {
        private DatabaseContext _context;

        public ItemRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Item> GetAll()
        {
            return _context.Items.ToList();
        }

        public IEnumerable<Item> GetEveryUnordered()
        {
            return _context.Items.Where(y => y.OrderId == null).ToList();
        }

        public IEnumerable<Item> SearchItemByName(string name)
        {
            return _context.Items.Where(y => y.Name.Contains(name)).ToList();
        }

        public Item? GetItem(int id)
        {
            return _context.Items.FirstOrDefault(items => items.Id == id);
        }

        public void AddItem(int id, string name, string description, string image, decimal price)
        {
            Item NewItem = new Item(0, name, description, image, price);
            _context.Items.Add(NewItem);
            _context.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            var item = _context.Items.FirstOrDefault(item => item.Id == id);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
        }

        public void ChangeItemData(int id, string name, string description, string image, decimal price)
        {
            var existingItem = _context.Items.FirstOrDefault(i => i.Id == id);
            existingItem.Id = id;
            existingItem.Name = name;
            existingItem.Price = price;
            existingItem.Description = description;
            existingItem.Image = image;
            _context.SaveChanges();
        }
    }
}