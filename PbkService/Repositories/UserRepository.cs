using PbkService.Data;
using PbkService.Models;

namespace PbkService.Repositories
{
    public class UserRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public User GetByUsername(string username)
        {
           return _context.Users.FirstOrDefault(x => x.Username == username);
        }

        public async Task Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
