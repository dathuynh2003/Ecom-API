using Ecom.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecom.Infrastructure.Extensions
{
    public static class ConfigureOptionsExtension
    {
        public static IServiceCollection AddConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            _ = services.Configure<DatabaseOptions>(configuration.GetSection(nameof(DatabaseOptions)));
            _ = services.Configure<PayOSOptions>(configuration.GetSection(nameof(PayOSOptions)));
            _ = services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
            return services;
        }
    }
}
