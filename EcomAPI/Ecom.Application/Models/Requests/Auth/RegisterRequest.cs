namespace Ecom.Application.Models.Requests.Auth
{
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly Dob { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
