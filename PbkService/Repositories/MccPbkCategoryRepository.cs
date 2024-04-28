using PbkService.Data;
using PbkService.Models;
using X.PagedList;

namespace PbkService.Repositories
{
    public class MccPbkCategoryRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public MccPbkCategory? GetByCategoryIdMccCode(int categoryId, string mccCode)
        {
            return _context.MccPbkCategories.FirstOrDefault(mc => mc.PbkCategoryId == categoryId && mc.MccCode == mccCode);
        }

        public IEnumerable<MccPbkCategory?> Get()
        {
            return [.. _context.MccPbkCategories];
        }

        public IEnumerable<MccPbkCategory?> GetByCategoryId(int categoryId)
        {
            return _context.MccPbkCategories.Where(mc => mc.PbkCategoryId == categoryId);
        }

        public IEnumerable<MccPbkCategory?> GetByMccCode(string mccCode)
        {
            return _context.MccPbkCategories.Where(mc => mc.MccCode == mccCode);
        }

        public void Create(MccPbkCategory mc)
        {
            _context.MccPbkCategories.Add(mc);
            _context.SaveChanges();
        }

        public void Delete(int categoryId, string mccCode)
        {
            MccPbkCategory? mc = GetByCategoryIdMccCode(categoryId, mccCode);
            if (mc == null)
            {
                return;
            }
            _context.MccPbkCategories.Remove(mc);
            _context.SaveChanges();
        }

        public void Delete(IEnumerable<MccPbkCategory?> mcs)
        {
            _context.MccPbkCategories.RemoveRange(mcs);
            _context.SaveChanges();
        }
    }
}
