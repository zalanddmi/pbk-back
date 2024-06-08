using PbkService.Data;
using PbkService.Models;
using X.PagedList;

namespace PbkService.Repositories
{
    public class OutletRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public Outlet? GetById(int id)
        {
            return _context.Outlets.FirstOrDefault(outlet => outlet.Id == id);
        }

        public Outlet? GetByName(string name)
        {
            return _context.Outlets.FirstOrDefault(outlet => outlet.Name.ToLower().Contains(name.ToLower()));
        }

        public IEnumerable<Outlet?> GetOutlets()
        {
            return [.. _context.Outlets];
        }

        public IPagedList<Outlet> GetPagedList(int pageNumber, int pageSize, string? searchString = null)
        {
            IQueryable<Outlet> query = _context.Outlets;
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(outlet => outlet.Name.ToLower().Contains(searchString)
                || outlet.Shop.Name.ToLower().Contains(searchString)
                || outlet.MccCode.ToLower().Contains(searchString));
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
