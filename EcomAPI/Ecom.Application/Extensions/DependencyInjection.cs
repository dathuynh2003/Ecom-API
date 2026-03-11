using Microsoft.Extensions.DependencyInjection;

namespace Ecom.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            _ = services.AddScoped<UseCases.Interfaces.IAuthenticationUseCase, UseCases.Implementations.AuthenticationUseCase>();
            _ = services.AddScoped<UseCases.Interfaces.IUserUseCase, UseCases.Implementations.UserUseCase>();
            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddUseCases();
            return services;
        }
    }
}
