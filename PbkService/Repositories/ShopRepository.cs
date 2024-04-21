using PbkService.Data;
using PbkService.Models;

namespace PbkService.Repositories
{
    public class ShopRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public Shop? GetShopById(int id)
        {
            return _context.Shops.FirstOrDefault(shop => shop.Id == id);
        }

        public IEnumerable<Shop?> GetShops()
        {
            return [.. _context.Shops];
        }

        public int Create(Shop shop)
        {
            _context.Shops.Add(shop);
            _context.SaveChanges();
            return shop.Id;
        }

        public void Update(Shop shop)
        {
            _context.Shops.Update(shop);
            _context.SaveChanges();
        }

        public void Delete(Shop shop)
        {
            _context.Shops.Remove(shop);
            _context.SaveChanges();
        }
    }
}
