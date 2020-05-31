using System.Linq;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Wallace.Application.Middleware;

namespace Wallace.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services
        )
        {
            return services
                .AddValidators(Assembly.GetExecutingAssembly())
                .AddTransient(
                    typeof(IPipelineBehavior<,>),
                    typeof(RequestValidationBehavior<,>)
                )
                .AddSingleton(
                    typeof(IPipelineBehavior<,>),
                    typeof(CacheRequestBehavior<,>)
                )
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Adds all the validators from all the commands and queries as a
        /// transient service.
        /// </summary>
        public static IServiceCollection AddValidators(
            this IServiceCollection services,
            Assembly assembly
        )
        {
            var validatorType = typeof(IValidator<>);

            var validatorTypes = assembly
                .GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == validatorType
                    )
                )
                .ToList();

            foreach (var validator in validatorTypes)
            {
                var requestType = validator
                    .GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() ==
                                typeof(IValidator<>))
                    .Select(i => i.GetGenericArguments()[0])
                    .First();

                // In the case of some commands they derive from a base class
                // instead of repeating fields (example: Account commands), in
                // those cases we need to retrieve all the assignable classes
                // from the request type and register those validators as well.
                var applicableTypes = assembly
                    .GetExportedTypes()
                    .Where(p => requestType.IsAssignableFrom(p));

                foreach (var type in applicableTypes)
                {
                    var validatorInterface = validatorType
                        .MakeGenericType(type);

                    services.AddTransient(validatorInterface, validator);
                }
            }

            return services;
        }
    }
}