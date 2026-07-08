using AmazeCareWebApi.Models;

namespace AmazeCareWebApi.Repository.Interface
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByUserNameAsync(string userName);
        Task CreateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);

    }
}
