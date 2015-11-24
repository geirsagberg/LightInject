using Microsoft.Extensions.DependencyInjection;

namespace LightInject.AspNet5
{
    internal class LightInjectServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceFactory factory;

        public LightInjectServiceScopeFactory(IServiceFactory factory)
        {
            this.factory = factory;
        }

        public IServiceScope CreateScope()
        {
            return new LightInjectServiceScope(factory);
        }
    }
}