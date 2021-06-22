using Microsoft.Extensions.DependencyInjection;

namespace KKIHub.Content.SyncService.Infrastructure.Dependency
{
    public interface IDependency
    {
        void Register(IServiceCollection services);
    }
}
