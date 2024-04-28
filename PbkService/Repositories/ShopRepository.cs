using PbkService.Data;
using PbkService.Models;
using X.PagedList;

namespace PbkService.Repositories
{
    public class ShopRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public Shop? GetById(int id)
        {
            return _context.Shops.FirstOrDefault(shop => shop.Id == id);
        }

        public IEnumerable<Shop?> GetShops()
        {
            return [.. _context.Shops];
        }

        public IPagedList<Shop> GetPagedList(int pageNumber, int pageSize, string? searchString = null)
        {
            IQueryable<Shop> query = _context.Shops;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(shop => shop.Name.Contains(searchString));
            }
            query = query.OrderBy(shop => shop.Id);
            return query.ToPagedList(pageNumber, pageSize);
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
