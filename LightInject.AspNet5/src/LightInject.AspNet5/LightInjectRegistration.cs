using System;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace LightInject.AspNet5
{
    public static class LightInjectRegistration
    {
        private static ILifetime ToLightInjectLifetime(this ServiceLifetime serviceLifetime)
        {
            switch (serviceLifetime) {
                case ServiceLifetime.Scoped:
                    return new PerScopeLifetime();
                case ServiceLifetime.Singleton:
                    return new PerContainerLifetime();
                case ServiceLifetime.Transient:
                default:
                    return new PerRequestLifeTime();
            }
        }

        public static IServiceProvider GetPopulatedServiceProvider(this IServiceContainer container, IServiceCollection services)
        {
            //container.RegisterFallback((type, name) => true, request => Activator.CreateInstance(request.ServiceType));
            container.Populate(services);
            var serviceProvider = new LightInjectServiceProvider(container);
            container.RegisterInstance<IServiceProvider>(serviceProvider);
            container.Register<IServiceScope>(factory => new LightInjectServiceScope(factory));
            container.Register<IServiceScopeFactory>(factory => new LightInjectServiceScopeFactory(factory));
            return serviceProvider;
        }

        private static void Populate(this IServiceRegistry serviceContainer, IServiceCollection services)
        {
            foreach (var descriptor in services) {
                var lifetime = descriptor.Lifetime.ToLightInjectLifetime();
                if (descriptor.ImplementationType != null) {
                    serviceContainer.Register(descriptor.ServiceType, descriptor.ImplementationType, lifetime);
                } else if (descriptor.ImplementationFactory != null) {
                    Func<IServiceFactory, object> expression = factory => descriptor.ImplementationFactory(factory.GetInstance<IServiceProvider>());

                    serviceContainer.Register(new ServiceRegistration {
                        ServiceType = descriptor.ServiceType,
                        Lifetime = lifetime,
                        FactoryExpression = expression,
                        ServiceName = ""
                    });
                    serviceContainer.Register(factory => descriptor.ImplementationFactory(factory.GetInstance<IServiceProvider>()), lifetime);
                } else {
                    serviceContainer.RegisterInstance(descriptor.ServiceType, descriptor.ImplementationInstance);
                }
            }
        }
    }
}