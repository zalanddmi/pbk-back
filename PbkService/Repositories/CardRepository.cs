using PbkService.Data;
using PbkService.Models;
using X.PagedList;

namespace PbkService.Repositories
{
    public class CardRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public Card? GetById(int id)
        {
            return _context.Cards.FirstOrDefault(card => card.Id == id);
        }

        public IEnumerable<Card?> Get()
        {
            return [.. _context.Cards];
        }

        public IPagedList<Card> GetPaged(int pageNumber, int pageSize, string? searchString = null)
        {
            IQueryable<Card> query = _context.Cards;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(card => card.Name.Contains(searchString)
                || card.Bank.Name.Contains(searchString)
                || card.TypeCard.Name.Contains(searchString));
            }
            query = query.OrderBy(card => card.Id);
            return query.ToPagedList(pageNumber, pageSize);
        }

        public int Create(Card card)
        {
            _context.Cards.Add(card);
            _context.SaveChanges();
            return card.Id;
        }

        public void Update(Card card)
        {
            _context.Cards.Update(card);
            _context.SaveChanges();
        }

        public void Delete(Card card)
        {
            _context.Cards.Remove(card);
            _context.SaveChanges();
        }
    }
}
