using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Specification;
using Xunit;

namespace LightInject.AspNet5.Tests
{
    public class LightInjectServiceProviderTests : DependencyInjectionSpecificationTests
    {
        protected override IServiceProvider CreateServiceProvider(IServiceCollection serviceCollection)
        {
            var container = new ServiceContainer();
            return container.GetPopulatedServiceProvider(serviceCollection);
        }
    } 
}