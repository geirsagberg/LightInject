using System;

namespace LightInject.AspNet5
{
    internal class LightInjectServiceProvider : IServiceProvider
    {
        private readonly IServiceFactory factory;

        public LightInjectServiceProvider(IServiceFactory factory)
        {
            this.factory = factory;
        }

        public object GetService(Type serviceType)
        {
            return factory.TryGetInstance(serviceType);
        }
    }
}