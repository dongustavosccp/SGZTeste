using AuthAPI.Application.Contracts;
using AuthAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUserName(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.NmUser == username) ?? new();
        }

        public async Task<bool> VerifyUserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.NmUser == username);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}