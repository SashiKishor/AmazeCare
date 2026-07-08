using AmazeCareWebApi.Dtos.User;
using AmazeCareWebApi.Models;

namespace AmazeCareWebApi.Services.Interface
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, LoginResponceDto? Data)> LoginAsync(LoginRequestDto loginRequestDto);
        Task<(bool Sucess, string Message)> CreateUserAsync(UserCreateDto user);
        Task<(bool Sucess, string Message, int? UserId)> CreateUsers(AdminAccessCreateDto doctor); 
    }
}
