using todo_list_back.DTOs;
using todo_list_back.Models;

namespace todo_list_back.Services
{
    public interface IAuthService
    {
        Task<User> Register(RegisterDto dto);
        Task<AuthResponseDto> Login(LoginDto dto);
    }
}
