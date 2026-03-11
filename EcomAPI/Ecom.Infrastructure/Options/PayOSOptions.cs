namespace Ecom.Infrastructure.Options
{
    public class PayOSOptions
    {
        public string ApiKey { get; set; }
        public string ClientId { get; set; }
        public string ChecksumKey { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public string WebhookUrl { get; set; }
    }
}
