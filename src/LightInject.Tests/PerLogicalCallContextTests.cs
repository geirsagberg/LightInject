using System;
using System.Threading.Tasks;
using LightInject.SampleLibrary;
using Xunit;

namespace LightInject.Tests
{
    public class PerLogicalCallContextTests : IDisposable
    {
        private readonly ServiceContainer container;
        private readonly Scope scope;

        public PerLogicalCallContextTests()
        {
            container = new ServiceContainer();
            container.Register<IFoo, Foo>(new PerScopeLifetime());
            container.ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider();

            scope = container.BeginScope();
        }

        private void DoInNewScope(Action action)
        {
            using (container.BeginScope())
            {
                Task.Delay(100).Wait();
                action();
            }
        }

        [Fact]
        public void Will_get_different_instances_in_different_scopes_async()
        {
            IFoo foo1 = null;
            IFoo foo2 = null;

            Task.WaitAll(
                Task.Run(() => DoInNewScope(() => foo1 = container.GetInstance<IFoo>())),
                Task.Run(() => DoInNewScope(() => foo2 = container.GetInstance<IFoo>()))
            );

            Assert.NotSame(foo1, foo2);
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}