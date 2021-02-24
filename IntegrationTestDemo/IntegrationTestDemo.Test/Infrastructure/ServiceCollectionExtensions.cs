using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
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

        public static Mock<T> MockService<T>(this IServiceCollection services, Action<Mock<T>> setupAction) where T : class
        {
            services.TryRemoveService<T>();
            var mock = new Mock<T>();
            setupAction?.Invoke(mock);
            services.AddSingleton(mock.Object);

            return mock;
        }

        public static Mock<T> MockService<T>(this IServiceCollection services, Mock<T> mock) where T : class
        {
            services.TryRemoveService<T>();
            services.AddSingleton(mock.Object);

            return mock;
        }
    }
}
