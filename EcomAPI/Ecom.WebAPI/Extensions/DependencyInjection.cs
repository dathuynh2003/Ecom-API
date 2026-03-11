namespace Ecom.WebAPI.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddCustomSwagger();
            _ = services.AddCustomCors();
            return services;
        }
    }
}
