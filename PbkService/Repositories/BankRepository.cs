using PbkService.Data;
using PbkService.Models;
using X.PagedList;

namespace PbkService.Repositories
{
    public class BankRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public Bank? GetById(int id)
        {
            return _context.Banks.FirstOrDefault(bank => bank.Id == id);
        }

        public IEnumerable<Bank?> GetBanks()
        {
            return [.. _context.Banks];
        }

        public IPagedList<Bank> GetPagedList(int pageNumber, int pageSize, string? searchString = null)
        {
            IQueryable<Bank> query = _context.Banks;
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(bank => bank.Name.ToLower().Contains(searchString));
            }
            query = query.OrderBy(bank => bank.Id);
            return query.ToPagedList(pageNumber, pageSize);
        }

        public int Create(Bank bank)
        {
            _context.Banks.Add(bank);
            _context.SaveChanges();
            return bank.Id;
        }
        
        public void Update(Bank bank)
        {
            _context.Banks.Update(bank);
            _context.SaveChanges();
        }

        public void Delete(Bank bank)
        {
            _context.Banks.Remove(bank);
            _context.SaveChanges();
        }
    }
}
