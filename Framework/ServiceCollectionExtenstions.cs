using Framework.Interface;
using Framework.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Framework
{
    public static class ServiceCollectionExtenstions
    {
        public static IServiceCollection AddFrameworkServices(this IServiceCollection services)
        {
            //services.AddScoped<ISendGridEmailService, SendGridEmailService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}