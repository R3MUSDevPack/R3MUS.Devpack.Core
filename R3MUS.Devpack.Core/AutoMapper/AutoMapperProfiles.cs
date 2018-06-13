using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace R3MUS.Devpack.Core.AutoMapper
{
    public class AutoMapperProfiles
    {
        public List<Type> LoadProfiles()
        {
            var assemblyName = Assembly.GetCallingAssembly();

            var types = new List<Type>();

            types.AddRange(GetLoadableTypes(assemblyName).Where(w => w.IsSubclassOf(typeof(Profile))).ToList());

            return types;
        }

        private IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
