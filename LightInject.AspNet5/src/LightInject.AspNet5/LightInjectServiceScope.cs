using System;
using Microsoft.Extensions.DependencyInjection;

namespace LightInject.AspNet5
{
    internal class LightInjectServiceScope : IServiceScope
    {
        private readonly IServiceFactory factory;
        private readonly Scope scope;
        public LightInjectServiceScope(IServiceFactory factory)
        {
            this.factory = factory;
            scope = this.factory.BeginScope();
        }

        public IServiceProvider ServiceProvider => new LightInjectServiceProvider(factory);

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}