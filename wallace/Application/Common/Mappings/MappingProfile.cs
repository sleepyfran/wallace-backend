using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Wallace.Application.Common.Mappings
{
    /// <summary>
    /// Automatically registers all the mappings available in the backend by
    /// looking for all the implementations of the IMapping interface.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => 
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapping<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = 
                    type.GetMethod("Mapping") ?? 
                    type.GetInterface("IMapFrom`1").GetMethod("Mapping");
                
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}