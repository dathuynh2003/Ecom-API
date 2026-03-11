using Ecom.Domain.Entities;
using Ecom.Domain.Enums;

namespace Ecom.Application.Models.Responses.User
{
    public class GetUserInfoResponse
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly Dob { get; set; }
        public string? AvatarUrl { get; set; }
        public string RoleName { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public Guid CartID { get; set; }
        //public Cart Cart { get; private set; }
    }
}
