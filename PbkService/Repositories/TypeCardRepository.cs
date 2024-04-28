using PbkService.Data;
using PbkService.Models;

namespace PbkService.Repositories
{
    public class TypeCardRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public TypeCard? GetById(int id)
        {
            return _context.TypeCards.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<TypeCard?> Get()
        {
            return [.. _context.TypeCards];
        }

        public int Create(TypeCard typeCard)
        {
            _context.TypeCards.Add(typeCard);
            _context.SaveChanges();
            return typeCard.Id;
        }

        public void Update(TypeCard typeCard)
        {
            _context.TypeCards.Update(typeCard);
            _context.SaveChanges();
        }

        public void Delete(TypeCard typeCard)
        {
            _context.TypeCards.Remove(typeCard);
            _context.SaveChanges();
        }
    }
}
