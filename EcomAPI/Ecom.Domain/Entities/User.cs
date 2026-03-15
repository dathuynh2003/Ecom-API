using Ecom.Domain.Base;
using Ecom.Domain.Enums;

namespace Ecom.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateOnly Dob { get; private set; }
        public string Password { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public bool IsActive { get; private set; } = false;
        public string? AvatarUrl { get; private set; }
        public UserRole Role { get; private set; }
        public ICollection<Address> Addresses { get; private set; }
        public Guid CartID { get; set; }
        public Cart Cart { get; set; }
        public ICollection<EmailVerificationToken> EmailVerificationTokens { get; private set; }

        public static User Create (string name, string email, string phone, DateOnly dob, string password, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("Invalid email");
            
            return new User { Name = name, Email = email, PhoneNumber = phone, Dob = dob, Password = password, Role = role };
        }

        public void Activate() => IsActive = true;
        public void ChangeRole(UserRole newRole) => Role = newRole;  // Chỉ nếu authorized
        public bool IsAdmin() => Role == UserRole.Admin;
        public bool IsCustomer() => Role == UserRole.Customer;
    }
}
