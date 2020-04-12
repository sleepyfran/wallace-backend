using Microsoft.Extensions.DependencyInjection;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Common.Interfaces;

namespace Wallace.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services
        )
        {
            return services
                .AddTransient<IPasswordHasher, PasswordHasher>()
                .AddTransient<IDateTime, DateTime>();
        }
    }
}