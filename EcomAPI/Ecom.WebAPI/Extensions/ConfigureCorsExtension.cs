namespace Ecom.WebAPI.Extensions
{
    public static class ConfigureCorsExtension
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            _ = services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    _ = policy.WithOrigins("http://localhost:3101") // Update with your frontend URL
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            return services;
        }
    }
}
