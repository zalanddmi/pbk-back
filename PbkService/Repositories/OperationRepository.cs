using PbkService.Data;
using PbkService.Models;
using X.PagedList;

namespace PbkService.Repositories
{
    public class OperationRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public Operation? GetById(int id)
        {
            return _context.Operations.FirstOrDefault(operation => operation.Id == id);
        }

        public IEnumerable<Operation?> Get()
        {
            return [.. _context.Operations];
        }

        public IPagedList<Operation> GetPagedList(int pageNumber, int pageSize, string? searchString = null)
        {
            IQueryable<Operation> query = _context.Operations;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(operation => operation.Outlet.Name.Contains(searchString));
            }
            query = query.OrderBy(operation => operation.Id);
            return query.ToPagedList(pageNumber, pageSize);
        }

        public void Create(Operation operation)
        {
            _context.Operations.Add(operation);
        }

        public void Create(IEnumerable<Operation> operations)
        {
            _context.Operations.AddRange(operations);
        }

        public void Update(Operation operation)
        {
            _context.Operations.Update(operation);
        }

        public void Delete(Operation operation)
        {
            _context.Operations.Remove(operation);
        }

        public void Delete(IEnumerable<Operation> operations)
        {
            _context.Operations.RemoveRange(operations);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
