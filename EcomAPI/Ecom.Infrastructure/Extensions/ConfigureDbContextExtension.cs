using Ecom.Infrastructure.Options;
using Ecom.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecom.Infrastructure.Extensions
{
    public static class ConfigureDbContextExtension
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseOptions = configuration.GetSection(nameof(DatabaseOptions)).Get<DatabaseOptions>();
            _ = services.AddDbContext<AppDbContext>(options =>
            {
                _ = options.UseNpgsql(databaseOptions?.ConnectionStrings);
                _ = options.EnableSensitiveDataLogging();
            });

            return services;
        }
    }
}
