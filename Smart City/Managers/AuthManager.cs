using AutoMapper;
using Smart_City.Dtos;
using Smart_City.Models;
using Smart_City.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace Smart_City.Managers
{
    public class AuthManager : IAuthManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthManager(IUserRepository userRepository, IMapper mapper, IConfiguration config)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _config = config;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var exists = await _userRepository.GetByNationalIdAsync(dto.NationalId);
            if (exists != null)
                return new AuthResponseDto { Success = false, Message = "User already exists" };

            var user = new User
            {
                Name = dto.Name,
                NationalId = dto.NationalId,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = "Citizen",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Address = dto.Address
            };

            await _userRepository.AddAsync(user);

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByNationalIdAsync(dto.NationalId);
            if (user == null)
                return new AuthResponseDto { Success = false, Message = "Invalid National ID" };

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return new AuthResponseDto { Success = false, Message = "Invalid password" };

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<AuthResponseDto> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return new AuthResponseDto { Success = false, Message = "User not found" };

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
                return new AuthResponseDto { Success = false, Message = "Old password is incorrect" };

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return new AuthResponseDto { Success = true, Message = "Password changed successfully" };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return new AuthResponseDto { Success = false, Message = "User not found" };

            var token = GenerateJwtToken(user);
            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                User = _mapper.Map<UserDto>(user)
            };
        }

        // =========  OTP FOR FORGOT PASSWORD  =========

        public async Task<bool> GeneratePasswordResetOtpAsync(string nationalId, string email)
        {
            var user = await _userRepository.GetByNationalIdAsync(nationalId);
            if (user == null)
                return false;

            // تأكيد إن الإيميل اللي دخله المستخدم هو نفس المسجل
            if (!string.Equals(user.Email?.Trim(), email?.Trim(), StringComparison.OrdinalIgnoreCase))
                return false;

            var random = new Random();
            var code = random.Next(100000, 999999).ToString(); // 6 digits

            user.PasswordResetOtp = code;
            user.PasswordResetExpiry = DateTime.UtcNow.AddMinutes(3); // OTP صالح 3 دقائق

            await _userRepository.UpdateAsync(user);

            await SendOtpEmailAsync(user.Email, code);

            return true;
        }

        public async Task<bool> ResetPasswordWithOtpAsync(string nationalId, string otp, string newPassword)
        {
            var user = await _userRepository.GetByNationalIdAsync(nationalId);
            if (user == null)
                return false;

            if (user.PasswordResetOtp != otp ||
                user.PasswordResetExpiry == null ||
                user.PasswordResetExpiry < DateTime.UtcNow)
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordResetOtp = null;
            user.PasswordResetExpiry = null;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        private async Task SendOtpEmailAsync(string toEmail, string otp)
        {
            var host = _config["Smtp:Host"];
            var port = int.Parse(_config["Smtp:Port"] ?? "587");
            var fromEmail = _config["Smtp:From"];
            var username = _config["Smtp:Username"];
            var password = _config["Smtp:Password"];

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(username, password)
            };

            var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = "Smart City - Password Reset OTP",
                Body = $"Your OTP code is: {otp}\nThis code is valid for 3 minutes.",
                IsBodyHtml = false
            };

            await client.SendMailAsync(message);
        }

        // =============================================

        private string GenerateJwtToken(User user)
        {
            var keyStr = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(keyStr))
                throw new InvalidOperationException("JWT Key is missing. Set 'Jwt:Key' in appsettings.json.");

            var issuer = _config["Jwt:Issuer"] ?? "SmartCity";
            var audience = _config["Jwt:Audience"] ?? "SmartCityClients";
            var expiryMinutes = int.TryParse(_config["Jwt:ExpiryMinutes"], out var mins) ? mins : 360;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("id", user.Id.ToString()),
                new Claim("nationalId", user.NationalId ?? string.Empty),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? "Citizen")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
