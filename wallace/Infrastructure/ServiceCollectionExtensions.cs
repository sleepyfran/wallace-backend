using Microsoft.Extensions.DependencyInjection;
using Wallace.Application.Common.Interfaces;

namespace Wallace.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPasswordHasher), typeof(PasswordHasher));
            return services;
;        }
    }
}