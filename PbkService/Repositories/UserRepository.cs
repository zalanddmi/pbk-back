using Microsoft.EntityFrameworkCore;
using PbkService.Data;
using PbkService.Models;

namespace PbkService.Repositories
{
    public class UserRepository
    {
        private readonly PbkContext _context;

        public UserRepository(PbkContext context)
        {
            _context = context;
        }

        public async Task Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
