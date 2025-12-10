using System;
using System.ComponentModel.DataAnnotations;

namespace Smart_City.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; }

        [Required, StringLength(14)]
        public string NationalId { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(15)]
        public string Phone { get; set; }

        [Required]
        public string Role { get; set; }

        [Required, StringLength(250)]
        public string Address { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;


        //OTP
        public string? PasswordResetOtp { get; set; }
        public DateTime? PasswordResetExpiry { get; set; }
    }
}
