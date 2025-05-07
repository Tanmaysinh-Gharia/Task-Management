using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.InjectionInterfaces;
using TaskManagement.Core.TypeFinder;

namespace TaskManagement.Core
{
    /// <summary>
    /// Responsible for registering dependencies across all layers dynamically
    /// using classes that implement IDependencyInjection interface.
    /// </summary>
    public static class DependencyRegistrar
    {
        /// <summary>
        /// Scans the application using the provided ITypeFinder to locate all implementations
        /// of IDependencyInjection, sorts them by their Order, and invokes their Register method.
        /// </summary>
        /// <param name="services">The IServiceCollection to register services into.</param>
        /// <param name="typeFinder">Used to find all types implementing IDependencyInjection.</param>
        /// <param name="configuration">Application configuration instance.</param>
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


        /// <summary>
        /// Registers all AutoMapper profiles available in the current application domain's assemblies.
        /// </summary>
        /// <param name="services">The IServiceCollection to register AutoMapper with.</param>
        public static void RegisterMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

    }
}
