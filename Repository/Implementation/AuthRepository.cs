using AmazeCareWebApi.Data;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AmazeCareWebApi.Repository.Implementation
{
    public class AuthRepository:IAuthRepository
    {
        private readonly AppoinmentDbContext _context;

        public AuthRepository(AppoinmentDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
                .Include(u => u.Doctor)
                .Include(u => u.Patient)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(a=>a.UserId==id);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            await _context.Users.Where(a=>a.UserId==id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

    }
}
