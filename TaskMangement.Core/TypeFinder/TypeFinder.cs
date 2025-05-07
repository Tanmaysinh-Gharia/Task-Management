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
            var visited = new HashSet<string>();
            var entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly == null)
                return assemblies;

            void LoadRecursively(Assembly assembly)
            {
                if (assembly == null || !visited.Add(assembly.FullName))
                    return;

                if (assembly.FullName.StartsWith("TaskManagement."))
                    assemblies.Add(assembly);

                foreach (var reference in assembly.GetReferencedAssemblies())
                {
                    if (!reference.FullName.StartsWith("TaskManagement."))
                        continue;
                    try
                    {
                        var referencedAssembly = Assembly.Load(reference);
                        LoadRecursively(referencedAssembly);
                    }
                    catch
                    {
                        // Skip assemblies that can't be loaded
                    }
                }
            }

            LoadRecursively(entryAssembly);

            return assemblies;
        }
    }
}
