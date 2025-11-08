using System.ComponentModel.DataAnnotations;

namespace Smart_City.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NationalId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }   
        public DateTime CreatedAt { get; set; }
    }

    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; } 
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string NewPassword { get; set; }
    }

    public class PromoteDto
    {
        [Required]
        public int UserId { get; set; }
    }
}