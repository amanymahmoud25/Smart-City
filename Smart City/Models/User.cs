using System.ComponentModel.DataAnnotations;

namespace Smart_City.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string NationalId { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // ====== FOR PASSWORD RESET (OTP) ======
        public string? PasswordResetOtp { get; set; }
        public DateTime? PasswordResetExpiry { get; set; }
    }
}
