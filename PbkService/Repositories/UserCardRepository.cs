using PbkService.Data;
using PbkService.Models;

namespace PbkService.Repositories
{
    public class UserCardRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public UserCard? Get(int cardId, int userId)
        {
            return _context.UserCards.FirstOrDefault(uc => uc.UserId == userId && uc.CardId == cardId);
        }

        public void Create(UserCard userCard)
        {
            _context.UserCards.Add(userCard);
        }

        public void Create(IEnumerable<UserCard> userCards)
        {
            _context.UserCards.AddRange(userCards);
        }

        public void Delete(UserCard userCard)
        {
            _context.UserCards.Remove(userCard);
        }

        public void Delete(IEnumerable<UserCard> userCards)
        {
            _context.UserCards.RemoveRange(userCards);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
