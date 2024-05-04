using PbkService.Data;
using PbkService.Models;
using X.PagedList;

namespace PbkService.Repositories
{
    public class CashbackRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public Cashback? GetById(int id)
        {
            return _context.Cashbacks.FirstOrDefault(cash => cash.Id == id);
        }

        public IEnumerable<Cashback?> Get()
        {
            return [.. _context.Cashbacks];
        }

        public IPagedList<Cashback> GetPaged(int pageNumber, int pageSize, string? searchString = null)
        {
            IQueryable<Cashback> query = _context.Cashbacks;
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(cash => cash.Card.Name.ToLower().Contains(searchString)
                || cash.PbkCategory.Name.ToLower().Contains(searchString));
            }
            query = query.OrderBy(cash => cash.Id);
            return query.ToPagedList(pageNumber, pageSize);
        }

        public int Create(Cashback cashback)
        {
            _context.Cashbacks.Add(cashback);
            _context.SaveChanges();
            return cashback.Id;
        }

        public void Update(Cashback cashback)
        {
            _context.Cashbacks.Attach(cashback);
            _context.SaveChanges();
        }

        public void Delete(Cashback cashback)
        {
            _context.Cashbacks.Remove(cashback);
            _context.SaveChanges();
        }

        public void Delete(IEnumerable<Cashback> cashbacks)
        {
            _context.Cashbacks.RemoveRange(cashbacks);
            _context.SaveChanges();
        }
    }
}
