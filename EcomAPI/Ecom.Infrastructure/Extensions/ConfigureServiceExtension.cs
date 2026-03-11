using Ecom.Application.Abstractions.Auth;
using Ecom.Application.Abstractions.Persistence;
using Ecom.Infrastructure.Persistence.Repositories;
using Ecom.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ecom.Infrastructure.Extensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //Repositories
            _ = services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            _ = services.AddScoped<IUserRepository, UserRepository>();

            // Services
            _ = services.AddScoped<IJwtService, JwtTokenService>();
            return services;
        }
    }
}
