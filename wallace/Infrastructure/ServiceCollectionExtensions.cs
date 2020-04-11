using Microsoft.Extensions.DependencyInjection;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Common.Interfaces;

namespace Wallace.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IDateTime, DateTime>();
            return services;
;       }
    }
}