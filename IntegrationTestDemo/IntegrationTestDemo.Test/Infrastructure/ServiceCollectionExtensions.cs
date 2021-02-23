using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace IntegrationTestDemo.Test.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection TryRemoveService<TService>(this IServiceCollection services)
        {
            var descriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(TService));
            if (descriptor != null)
                services.Remove(descriptor);

            return services;
        }

        public static IServiceCollection TryRemoveService(this IServiceCollection services, string typeName)
        {
            var descriptor = services.FirstOrDefault(sd => sd.ServiceType?.Name == typeName);
            if (descriptor != null)
                services.Remove(descriptor);

            return services;
        }
    }
}
