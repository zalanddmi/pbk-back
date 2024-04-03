using Microsoft.EntityFrameworkCore;
using PbkService.Data;
using PbkService.Models;

namespace PbkService.Repositories
{
    public class UserRepository(PbkContext context)
    {
        private readonly PbkContext _context = context;

        public async Task<User?> GetByUsername(string username)
        {
           return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByPhoneNumber(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        }

        public async Task Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
