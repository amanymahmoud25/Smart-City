using Smart_City.Dtos;

namespace Smart_City.Managers
{
    public interface IAuthManager
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<AuthResponseDto> RefreshTokenAsync(int userId);

        // OTP - Forgot Password
        Task<bool> GeneratePasswordResetOtpAsync(string nationalId, string email);
        Task<bool> ResetPasswordWithOtpAsync(string nationalId, string otp, string newPassword);
    }
}
