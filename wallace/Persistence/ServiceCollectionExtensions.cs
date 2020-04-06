using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wallace.Application.Common.Interfaces;

namespace Wallace.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<WallaceDbContext>(options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString("WallaceDb"));
                });

            services.AddScoped<IDbContext>(provider => provider.GetService<WallaceDbContext>());

            return services;
        }
    }
}
