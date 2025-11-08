using Smart_City.Dtos;
using System.Threading.Tasks;

namespace Smart_City.Managers
{
    public interface IAuthManager
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<AuthResponseDto> RefreshTokenAsync(int userId);
    }
}
