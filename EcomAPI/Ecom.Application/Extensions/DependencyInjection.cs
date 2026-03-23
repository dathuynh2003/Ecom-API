using Microsoft.Extensions.DependencyInjection;

namespace Ecom.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            _ = services.AddScoped<UseCases.Interfaces.IAuthenticationUseCase, UseCases.Implementations.AuthenticationUseCase>();
            _ = services.AddScoped<UseCases.Interfaces.IUserUseCase, UseCases.Implementations.UserUseCase>();
            _ = services.AddScoped<UseCases.Interfaces.IBrandUseCase, UseCases.Implementations.BrandUseCase>();
            _ = services.AddScoped<UseCases.Interfaces.ICategoryUseCase, UseCases.Implementations.CategoryUseCase>();
            _ = services.AddScoped<UseCases.Interfaces.IProductUseCase, UseCases.Implementations.ProductUseCase>();
            _ = services.AddScoped<UseCases.Interfaces.ICategorySpecificationKeyUseCase, UseCases.Implementations.CategorySpecificationKeyUseCase>();
            _ = services.AddScoped<UseCases.Interfaces.ISpecificationKeyUseCase, UseCases.Implementations.SpecificationKeyUseCase>();
            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddUseCases();
            return services;
        }
    }
}
