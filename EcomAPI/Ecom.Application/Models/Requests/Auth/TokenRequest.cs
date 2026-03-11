namespace Ecom.Application.Models.Requests.Auth
{
    public class TokenRequest
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
