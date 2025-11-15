namespace Smart_City.Dtos
{
    public class ForgotPasswordDto
    {
        public string NationalId { get; set; }
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        public string NationalId { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
    }
}
