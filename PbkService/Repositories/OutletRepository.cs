using PbkService.Data;
using PbkService.Models;
using X.PagedList;

namespace PbkService.Repositories
{
    public class OutletRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public Outlet? GetOutletById(int id)
        {
            return _context.Outlets.FirstOrDefault(outlet => outlet.Id == id);
        }

        public IEnumerable<Outlet?> GetOutlets()
        {
            return [.. _context.Outlets];
        }

        public IPagedList<Outlet> GetPagedOutlets(int pageNumber, int pageSize, string? searchString = null)
        {
            IQueryable<Outlet> query = _context.Outlets;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(outlet => outlet.Name.Contains(searchString)
                || outlet.Shop.Name.Contains(searchString)
                || outlet.MccCode.Contains(searchString));
            }
            query = query.OrderBy(outlet => outlet.Id);
            return query.ToPagedList(pageNumber, pageSize);
        }

        public int Create(Outlet outlet)
        {
            _context.Outlets.Add(outlet);
            _context.SaveChanges();
            return outlet.Id;
        }

        public void Update(Outlet outlet)
        {
            _context.Outlets.Update(outlet);
            _context.SaveChanges();
        }

        public void Delete(Outlet outlet)
        {
            _context.Outlets.Remove(outlet);
            _context.SaveChanges();
        }
    }
}
