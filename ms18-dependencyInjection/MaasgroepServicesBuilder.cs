using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Maasgroep.DependencyInjection
{
    public class MaasgroepServicesBuilder
    {
        public IHost Iets(HostApplicationBuilder builder)
        {
            GetClassesWithAttribute(builder);

            return builder.Build();
        }

        internal void GetClassesWithAttribute(HostApplicationBuilder builder)
        {
            var allStaticAssemblies = GetAssemblies().Where(a => !a.IsDynamic);
            var allClassesWithAttribute = new List<ServiceAttributeClass>();

            foreach (var a in allStaticAssemblies)
            {
                foreach (var t in a.GetExportedTypes())
                {
                    if (t.GetCustomAttribute<ServiceAttribute>() != null)
                    {
                        var assembly = new ServiceAttributeClass()
                        {
                            LifeTime = t.GetCustomAttribute<ServiceAttribute>()?.Lifetime
                        ,   ServiceType = t
                        ,   ImplementationType = t.GetCustomAttribute<ServiceAttribute>()?.ServiceType
                        };

                        if (assembly.LifeTime != null)
                            allClassesWithAttribute.Add(assembly);
                    }
                }
            }

            foreach (var c in allClassesWithAttribute)
            {
                builder.Services.AddScoped(c.ImplementationType, c.ServiceType);
            }
        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            var list = new List<string>();
            var stack = new Stack<Assembly>();

            stack.Push(Assembly.GetEntryAssembly());

            do
            {
                var asm = stack.Pop();

                yield return asm;

                foreach (var reference in asm.GetReferencedAssemblies())
                    if (!list.Contains(reference.FullName))
                    {
                        stack.Push(Assembly.Load(reference));
                        list.Add(reference.FullName);
                    }

            }
            while (stack.Count > 0);
        }
    }
}
