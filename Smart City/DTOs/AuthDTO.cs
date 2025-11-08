using System.ComponentModel.DataAnnotations;
using FluentValidation;
namespace Smart_City.Dtos
{
    public class RegisterDto
    {
        [Required, StringLength(120)]
        public string Name { get; set; }

        [Required, StringLength(16, MinimumLength = 16, ErrorMessage = "National ID must be 16 digits.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "National ID must be 16 digits.")]
        public string NationalId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        [Required, MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }

        [Required, StringLength(250)]
        public string Address { get; set; }
    }

    public class LoginDto
    {
        [Required, StringLength(16, MinimumLength = 16)]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "National ID must be 16 digits.")]
        public string NationalId { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    public class RegisterDtoValidator : FluentValidation.AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
            RuleFor(x => x.NationalId).NotEmpty().Length(16).Matches(@"^\d{16}$");
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(250);
        }
    }
    public class LoginDtoValidator : FluentValidation.AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.NationalId).NotEmpty().Length(16).Matches(@"^\d{16}$");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }
}
