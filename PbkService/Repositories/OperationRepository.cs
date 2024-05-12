using PbkService.Data;
using PbkService.Models;

namespace PbkService.Repositories
{
    public class OperationRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public Operation? GetById(int id)
        {
            return _context.Operations.FirstOrDefault(operation => operation.Id == id);
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
