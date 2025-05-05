using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.TypeFinder
{
    public class TypeFinder : ITypeFinder
    {
        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof(T), onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType(
            Type assignTypeFrom, 
            bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(assignTypeFrom, GetAssemblies(), onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType
            (Type assignTypeFrom,
            IEnumerable<Assembly> assemblies,
            bool onlyConcreteClasses = true)
        {
            List<Type> results = [];

            try
            {
                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes()
                        .Where(type => assignTypeFrom.IsAssignableFrom(type) && !type.IsAbstract);
                    if (onlyConcreteClasses)
                    {
                        types = types.Where(type => type.IsClass);
                    }
                    results.AddRange(types);
                }
                // Remove duplicates
                return results.Distinct().ToList();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // Handle the exception as needed
                Console.WriteLine($"Error loading types: {ex.Message}");
                throw ex;
            }
        }


        public virtual IList<Assembly> GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            // Load referenced JWTAuth assemblies if not already loaded
            var requiredPrefixes = new[] { "TaskManagement.Bussiness", "TaskManagement.Data" };

            foreach (var assembly in loadedAssemblies)
            {
                assemblies.Add(assembly);

                foreach (var reference in assembly.GetReferencedAssemblies())
                {
                    if (requiredPrefixes.Any(p => reference.FullName.StartsWith(p)) &&
                        !loadedAssemblies.Any(a => a.FullName == reference.FullName))
                    {
                        var loaded = Assembly.Load(reference);
                        assemblies.Add(loaded);
                    }
                }
            }

            // Add manually if still missing
            foreach (var prefix in requiredPrefixes)
            {
                if (!assemblies.Any(a => a.FullName.StartsWith(prefix)))
                {
                    assemblies.Add(Assembly.Load(prefix));
                }
            }

            return assemblies.Distinct().ToList();
        }
    }
}
