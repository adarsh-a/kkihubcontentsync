using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace KKIHub.Content.SyncService.Infrastructure.Dependency
{
    public sealed class DependencyManager
    {
        private static readonly Lazy<DependencyManager> Instance = new Lazy<DependencyManager>(() => new DependencyManager(), LazyThreadSafetyMode.ExecutionAndPublication);

        private DependencyManager()
        {

        }

        public static DependencyManager DependencyInstance => Instance.Value;

        public void BuildDependencies(IServiceCollection services)
        {
            var type = typeof(IDependency);

            var typeInfo = type.GetTypeInfo();

            var instances = typeInfo
                .Assembly
                .GetExportedTypes()
                .Where(t => type.IsAssignableFrom(t) && t.GetTypeInfo().IsClass)
                .Select(t => Activator.CreateInstance(t) as IDependency)
                .ToList();

            instances.ForEach(x => x.Register(services));
        }
    }
}
