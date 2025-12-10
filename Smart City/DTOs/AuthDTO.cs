using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Smart_City.Dtos
{
    // ========== Register ==========
    public class RegisterDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(120, ErrorMessage = "Name cannot exceed 120 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "National ID is required.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "National ID must be exactly 14 digits.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be 14 digits.")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?\d{8,15}$",
            ErrorMessage = "Phone must be digits only and between 8–15 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; }
    }

    // ========== Login ==========
    public class LoginDto
    {
        [Required,
         StringLength(14, MinimumLength = 14),
         RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be 14 digits.")]
        public string NationalId { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }
    }

    // ========== Auth Response ==========
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    // ========== FluentValidation ==========
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(120);

            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required.")
                .Length(14).WithMessage("National ID must be exactly 14 digits.")
                .Matches(@"^\d{14}$").WithMessage("National ID must contain only digits.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress();

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .Matches(@"^\+?\d{8,15}$")
                .WithMessage("Phone must be digits only (8–15 digits)");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(250);
        }
    }

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required.")
                .Length(14).WithMessage("National ID must be exactly 14 digits.")
                .Matches(@"^\d{14}$").WithMessage("National ID must contain only digits.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.");
        }
    }
}
