using Microsoft.Extensions.DependencyInjection;
using Wallace.Domain.Identity;
using Wallace.Domain.Identity.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<JwtConfiguration>()
                .AddTransient<ITokenData, TokenData>()
                .AddTransient<ITokenBuilder, TokenBuilder>()
                .AddTransient<ITokenChecker, TokenChecker>()
                .AddScoped<IIdentityLoader, IdentityLoader>()
                .AddScoped<IdentityContainer>()
                .AddScoped<IIdentityAccessor>(s =>
                    s.GetRequiredService<IdentityContainer>()
                )
                .AddScoped<IIdentitySetter>(s =>
                    s.GetRequiredService<IdentityContainer>()
                );
        }
    }
}