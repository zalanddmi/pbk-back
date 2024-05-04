using PbkService.Data;
using PbkService.Models;
using X.PagedList;

namespace PbkService.Repositories
{
    public class PbkCategoryRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public PbkCategory? GetById(int id)
        {
            return _context.PbkCategories.FirstOrDefault(bank => bank.Id == id);
        }

        public IEnumerable<PbkCategory?> GetPbkCategories()
        {
            return [.. _context.PbkCategories];
        }

        public IPagedList<PbkCategory> GetPagedList(int pageNumber, int pageSize, string? searchString = null)
        {
            IQueryable<PbkCategory> query = _context.PbkCategories;
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(category => category.Name.ToLower().Contains(searchString));
            }
            query = query.OrderBy(category => category.Id);
            return query.ToPagedList(pageNumber, pageSize);
        }

        public int Create(PbkCategory category)
        {
            _context.PbkCategories.Add(category);
            _context.SaveChanges();
            return category.Id;
        }

        public void Update(PbkCategory category)
        {
            _context.PbkCategories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(PbkCategory category)
        {
            _context.PbkCategories.Remove(category);
            _context.SaveChanges();
        }
    }
}
