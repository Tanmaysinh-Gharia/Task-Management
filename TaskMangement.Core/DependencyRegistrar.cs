using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.InjectionInterfaces;
using TaskManagement.Core.TypeFinder;
using Mapster;
using MapsterMapper;
namespace TaskManagement.Core
{
    public static class DependencyRegistrar
    {
        public static void RegisterDependencies(
            this IServiceCollection services, 
            ITypeFinder typeFinder, 
            IConfiguration configuration)
        {
            // find dependency registrars defined in the assembly 
            IEnumerable<Type> dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyInjection>(true);

            //create and sort instances of dependency registrars
            IOrderedEnumerable<IDependencyInjection> instances = dependencyRegistrars.
                Select(type => (IDependencyInjection)Activator.CreateInstance(type))
                .OrderBy(instance => instance.Order);

            foreach (IDependencyInjection dependencyInjection in instances)
            {
                // Register the dependencies using the current instance
                dependencyInjection.Register(services, configuration);
            }
        }

        public static void RegisterMappings(this IServiceCollection services, ITypeFinder typeFinder)
        {
            //to map nested objects
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

            //to ignore null values while mapping
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);

            //configure mapster
            services.AddSingleton(TypeAdapterConfig.GlobalSettings);

            //add mapper to map object using IMapper
            services.AddScoped<IMapper, Mapper>();

            //find mapper profile types configurations provided by other assemblies
            IEnumerable<Type> mapperProfileTypes = typeFinder.FindClassesOfType<IMapperProfile>();

            //create instances of mapper profiles
            IEnumerable<IMapperProfile> mapperProfiles = mapperProfileTypes
                .Select(m => (IMapperProfile)Activator.CreateInstance(m));

            //add profiles
            foreach (IMapperProfile profile in mapperProfiles)
            {
                profile?.MapProfile();
            }
        }

    }
}
