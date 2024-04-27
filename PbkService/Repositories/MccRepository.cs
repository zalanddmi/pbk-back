using PbkService.Data;
using PbkService.Models;
using X.PagedList;

namespace PbkService.Repositories
{
    public class MccRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public Mcc? GetMccByCode(string code)
        {
            return _context.MCCs.FirstOrDefault(mcc => mcc.Code == code);
        }

        public List<Mcc> GetMccs()
        {
            return [.. _context.MCCs];
        }

        public IPagedList<Mcc> GetPagedMccs(int pageNumber, int pageSize, string? searchString = null)
        {
            IQueryable<Mcc> query = _context.MCCs;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(mcc => mcc.Name.Contains(searchString) || mcc.Code.Contains(searchString));
            }
            query = query.OrderBy(mcc => mcc.Code);
            return query.ToPagedList(pageNumber, pageSize);
        }

        public void Create(Mcc mcc)
        {
            _context.MCCs.Add(mcc);
        }

        public void Update(Mcc mcc)
        {
            _context.MCCs.Update(mcc);
        }

        public void Delete(Mcc mcc)
        {
            _context.MCCs.Remove(mcc);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
